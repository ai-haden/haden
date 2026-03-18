You are continuing development on the `haden` solution in this workspace:

- Repo root: `C:\ame\aiventure\aiventure-github\ai-haden\haden`
- Solution: `Haden.Autonomy.sln`
- Main app run command (Windows UI): `dotnet run --project Haden.NxtRemote\Haden.NxtRemote.csproj`

Current priorities:
1. Keep simulation and branch-01/branch-02 logic fully testable on Linux via console tests.
2. Preserve/extend Windows hardware Bluetooth COM test coverage for NXT.
3. Prepare for NXT brick replacement (current brick has screen issue; Linux Bluetooth pairing may require a new brick).
4. Preserve academic traceability (`docs/paper/light_seeker_branch12.tex`).

Important current context:
- `Haden.NxtRemote` is WinForms (`net9.0-windows`) and stays Windows-only.
- Cross-platform logic is in `Haden.Library`.
- New Linux-friendly simulation test project exists:
  - `Haden.ConsoleTests/Haden.ConsoleTests.csproj`
  - `Haden.ConsoleTests/Branch12ConsoleTests.cs`
  - `Haden.ConsoleTests/RewardConsoleTests.cs`
- New console hardware tests exist in `Haden.Tests`:
  - `Haden.Tests/HardwareSmokeTests.cs`
  - `Haden.Tests/HardwareLightSeekingConsoleTests.cs`
- `SanityTests` is marked hardware/manual explicit.
- Hardware tests are gated by Windows + COM and should skip gracefully.
- COM port env var for hardware tests: `HADEN_NXT_COM_PORT` (example: `COM40`).
- `global.json` pins SDK to `9.0.100`.
- `Haden.ConsoleTests` currently runs successfully; `Haden.Tests` restore can still fail on some setups due to SDK resolver/workload issues.

What to do first in the new session:
1. Check repo status, active branch, and recent commits.
2. Run Linux-friendly tests first:
   - `dotnet test Haden.ConsoleTests\Haden.ConsoleTests.csproj --logger "console;verbosity=detailed"`
3. Run Windows hardware console tests only when hardware is available:
   - `$env:HADEN_NXT_COM_PORT="COM40"`
   - `dotnet test Haden.Tests\Haden.Tests.csproj --filter "FullyQualifiedName~Haden.Tests.HardwareSmokeTests.NxtBluetoothSmoke_ConnectReadMoveDisconnect" --logger "console;verbosity=detailed"`
   - `dotnet test Haden.Tests\Haden.Tests.csproj --filter "FullyQualifiedName~Haden.Tests.HardwareLightSeekingConsoleTests.ConsoleLightSeeking_PerformsBoundedSeekCycle" --logger "console;verbosity=detailed"`
4. If `Haden.Tests` fails silently, rerun with diagnostic logging and capture exact `MSB` lines.
5. Update `CHANGELOG.md` for every code fix.

Guardrails:
- Do not revert unrelated local changes.
- Do not run destructive git commands.
- Prefer minimal, testable changes.
- Keep Linux test work in `Haden.ConsoleTests` (`net9.0`, no WinForms/NXT UI dependencies).
- Keep hardware tests isolated and explicitly environment-gated.

Expected output style:
- Findings first, then change summary.
- Include exact file paths and line references.
- Include runnable validation commands.
