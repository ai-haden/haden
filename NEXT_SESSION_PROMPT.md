You are continuing development on `haden` in this workspace:

- Repo root: `/home/cartheur/ame/aiventure/aiventure-github/ai-haden/haden`
- Active branch: `main` (Linux-first)
- Legacy branch: `windows-legacy` (reference only)

Primary mission for this next session:
1. Validate that real robot behavior no longer gets pathologically stuck in one direction when light evidence is uncertain.
2. Run full genuine RL episodes (terminate on bump sensor), not arbitrary early stops.
3. Capture paper-ready evidence showing `sense -> decide -> move` behavior with boredom-triggered directional reversal.

Current behavior state (already implemented):
- Triplet probe sensing (`L/C/R`) on sensor motor A before movement.
- Evidence-gated wheel movement (B/C) with confidence threshold.
- Adaptive probe escalation when contrast is flat.
- Boredom/anti-pathology policy:
  - direction bias alternation under sustained uncertainty,
  - boredom-triggered flip,
  - same-direction stuck forced flip,
  - near-peak now requires confirmed peak stability.
- Post-decision scan-head nudge disabled by default to avoid end-stop banging (`HADEN_SEEK_SCAN_NUDGE_ENABLE=0` default).
- Motor safety:
  - global best-effort de-power in `finally` for A/B/C,
  - manual `--all-stop` mode.

Important observed facts from this session:
- Robot previously showed pathological CCW-end behavior.
- New logs now include `actionMode`, `boredomBiasDir`, `boredomTriggered`, `forcedFlip`, `sameDirStuckCount`, `nearPeakConfirmed`.
- `--all-stop` successfully quieted motors.
- Bluetooth/RFCOMM can become stale (`Device or resource busy`); rebinding may be needed.

Session workflow:
1. Preflight + quality gate:
   - `dotnet build Haden.Autonomy.sln -warnaserror -v minimal`
   - `dotnet test Haden.Autonomy.sln --logger "console;verbosity=minimal"`
2. Hardware link readiness:
   - Verify `/dev/rfcomm0` is usable.
   - If needed: `sudo rfcomm release 0 || true` then `sudo rfcomm bind 0 00:16:53:17:9B:47 1`
   - Run safety command first: `dotnet run --project Haden.HardwareSmoke/Haden.HardwareSmoke.csproj -- --all-stop`
3. Live experiments:
   - Run `--seek-max-light` with bump termination enabled.
   - Test under normal, artificially dimmed, and perturbed lighting.
   - Ensure robot does not persist in one-direction pathology under uncertainty.
4. Data and paper outputs:
   - Extract episode-level summary: stop reason, iterations, reward, peak, recovery events.
   - Report percentages/counts of `seek`, `hold-uncertain`, `explore-bias`, `explore-flip`, `explore-forced-flip`.
   - Capture at least one clear case showing uncertainty -> flip -> improvement.
5. If pathology remains:
   - tune `HADEN_PEAK_CONFIRM_TICKS`, `HADEN_STUCK_SAME_DIR_LIMIT`, `HADEN_UNCERTAIN_ALTERNATE_STEPS`,
     `HADEN_BOREDOM_FLAT_LIMIT`, `HADEN_BOREDOM_UNCERTAIN_LIMIT`.

Deliverables required at end of next session:
- Exact code/doc changes with file references.
- Build/test results.
- Hardware run summaries with concrete telemetry evidence.
- Clear verdict: pathology solved or not solved, with next tuning step.

Guardrails:
- Keep Linux-first, headless workflow.
- Do not modify `windows-legacy`.
- Update `CHANGELOG.md` for every approved code fix.
- Between runs, keep motors de-powered (`--all-stop`).
