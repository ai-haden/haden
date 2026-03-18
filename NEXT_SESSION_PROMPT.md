You are continuing development on the `haden` solution in this workspace:

- Repo root: `/home/cartheur/ame/aiventure/aiventure-github/ai-haden/haden`
- Solution: `Haden.Autonomy.sln`

Current priorities:
1. Keep branch-01/branch-02 embodied and RL logic testable on Linux.
2. Keep cross-platform logic in `Haden.Library`.
3. Keep extracted migration candidates in `Haden.LinuxMigrationReview`.

Important current context:
- Linux console tests:
  - `Haden.ConsoleTests/Haden.ConsoleTests.csproj`
  - `Haden.ConsoleTests/Branch12ConsoleTests.cs`
  - `Haden.ConsoleTests/RewardConsoleTests.cs`
  - `Haden.ConsoleTests/AdaptiveSeekCycleTests.cs`
- Linux migration review library:
  - `Haden.LinuxMigrationReview/Haden.LinuxMigrationReview.csproj`
  - `Haden.LinuxMigrationReview/AdaptiveSeekCycle.cs`
- SDK pin:
  - `global.json` pins `9.0.100`

What to do first in the new session:
1. Check repo status, active branch, and recent commits.
2. Run Linux tests:
   - `dotnet test Haden.ConsoleTests/Haden.ConsoleTests.csproj --logger "console;verbosity=detailed"`
3. Update `CHANGELOG.md` for every code fix.

Guardrails:
- Do not revert unrelated local changes.
- Do not run destructive git commands.
- Prefer minimal, testable changes.
- Keep branch changes Linux-focused.
