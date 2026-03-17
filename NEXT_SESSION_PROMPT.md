You are continuing development on the `haden` solution in this workspace:

- Repo root: `C:\ame\aiventure\aiventure-github\ai-haden\haden`
- Solution: `Haden.Autonomy.sln`
- Main app run command: `dotnet run --project Haden.NxtRemote\Haden.NxtRemote.csproj`

Current priorities:
1. Prepare for reliable live hardware testing with NXT over Bluetooth serial.
2. Keep branch-01/branch-02 embodied + motivational logic testable in simulation.
3. Preserve academic traceability for paper writing (LaTeX docs under `docs/paper`).

Important current context:
- `Haden.NxtRemote` is WinForms (`net9.0-windows`) and runs on Windows.
- Cross-platform logic is in `Haden.Library`.
- Hardware tests should be Windows/COM-gated and must skip gracefully when no COM port exists.
- Logging now auto-creates `logs`/`db` directories.
- Existing embodied/motivational model:
  - `Haden.Library/Algorithm/IdealEmbodiedLightSeeker.cs`
  - `Haden.Tests/IdealBranch12LightSeekerTests.cs`
- Existing RL model/tests:
  - `Haden.Library/Algorithm/IdealReinforcementModel.cs`
  - `Haden.Tests/RewardTests.cs`
- Academic write-up:
  - `docs/paper/light_seeker_branch12.tex`

What to do first in the new session:
1. Check current repo status and recent commits.
2. Build test projects and report exact failures with file/line references.
3. Create/validate a Windows-only hardware smoke test that does:
   - Bluetooth COM connect
   - light sensor read loop
   - one motor movement command
   - clean disconnect
4. Keep simulation tests green while isolating hardware integration tests.
5. Update `CHANGELOG.md` for every code fix.

Guardrails:
- Do not revert unrelated local changes.
- Do not run destructive git commands.
- Prefer minimal, testable changes.
- If a command fails silently in this environment, rerun with alternate verbosity and capture useful diagnostics.

Expected output style:
- Findings first (if reviewing/fixing failures), then change summary.
- Include exact file paths and line references.
- Include runnable commands for validation.
