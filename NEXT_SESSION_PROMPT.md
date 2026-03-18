You are continuing development on `haden` in this workspace:

- Repo root: `/home/cartheur/ame/aiventure/aiventure-github/ai-haden/haden`
- Active branch target: `main` (Linux-first)
- Legacy reference branch: `windows-legacy` (frozen; read-only reference only)

Primary mission for this session:
1. Prepare live NXT hardware validation on this Linux test machine by re-pairing/replacing the brick Bluetooth mapping.
2. Develop and validate light-seeking behavior on real hardware and simulation.
3. Produce publication-grade experimental artifacts and writing inputs aimed at a world-class paper.

Critical context:
- Headless Linux projects:
  - `Haden.NxtSDK` (NXT protocol/transport)
  - `Haden.RobotBehavior` (behavior logic)
  - `Haden.Simulation` (whirl-driven simulation engine)
  - `Haden.ConsoleTests` (all tests)
- `windows-legacy` exists only for fallback comparison of old behavior.

Session workflow:
1. Verify build quality gate:
   - `dotnet build Haden.Autonomy.sln -warnaserror -v minimal`
   - `dotnet test Haden.Autonomy.sln --logger "console;verbosity=detailed"`
2. Hardware readiness:
   - Confirm Bluetooth pairing path for the new/reconfigured NXT brick.
   - Confirm serial transport visibility for NXT communication on Linux.
   - Add/adjust hardware-gated console validation for connect/read/turn/disconnect using `Haden.NxtSDK`.
3. Behavior development:
   - Improve light-seeking policy in `Haden.RobotBehavior` using measurable objectives (time-to-peak, stability, recovery after perturbation).
   - Keep behavior explainable and testable.
4. Simulation + real-world alignment:
   - Use `Haden.Simulation` to reproduce expected whirl/decision trajectories before hardware runs.
   - Compare sim vs hardware outcomes and log divergences.
5. Paper-oriented outputs:
   - Capture reproducible metrics, experiment configs, and ablation notes.
   - Preserve results in a form directly usable for manuscript figures/tables.

Deliverables required at end of session:
- Exact code changes with file references.
- Test/build results and hardware validation results.
- Quantitative behavior metrics and what improved/regressed.
- Clear next experimental step toward publication-quality evidence.

Guardrails:
- Do not use or modify `windows-legacy` except as reference.
- Keep all new logic Linux-first and headless.
- Keep changes minimal, test-backed, and reproducible.
- Update `CHANGELOG.md` for every code fix.
