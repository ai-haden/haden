# haden

The Framework mobile AI.

## Branch Policy

- `main` is Linux-first and contains the active codebase.
- `windows-legacy` is a frozen branch kept only for historical WinForms validation/comparison.
- Do not add new feature work to `windows-legacy`.

## Working here

* `dotnet test Haden.ConsoleTests/Haden.ConsoleTests.csproj --logger "console;verbosity=detailed"`
* `dotnet run --project Haden.HardwareSmoke/Haden.HardwareSmoke.csproj -- /dev/rfcomm0`
* `sudo rfcomm connect /dev/rfcomm0 00:16:53:17:9B:47 1`

## Each testing day

`sudo rfcomm connect /dev/rfcomm0 00:16:53:17:9B:47 1`

## Quality Gate

- `dotnet build Haden.Autonomy.sln -warnaserror -v minimal`
- `dotnet test Haden.Autonomy.sln --logger "console;verbosity=detailed"`

## Embodied Light-Seeker Decision Tree Export

The branch-01/branch-02 embodied model can be exported into a `TreeNode` view for explainability and paper figures.

- Model source: `Haden.Library/Algorithm/IdealEmbodiedLightSeeker.cs`
- Export adapter: `Haden.Library/Algorithm/IdealEmbodiedDecisionTreeAdapter.cs`
- Paper section (LaTeX): `docs/paper/light_seeker_branch12.tex`

Use this when you want a human-readable summary of learned `experiment -> result` interactions.
It is intended for interpretation and analysis, not as the primary control loop.

Example:

```csharp
using Haden.Library.Algorithm;
using Haden.Library.DecisionTree;

var seeker = new IdealEmbodiedLightSeeker(
    experiments: new[] { "scan-left", "scan-right" },
    initialExperiment: "scan-left");

seeker.SetResultValence("dark", -1.0);
seeker.SetResultValence("bright", 1.0);

seeker.Step(exp => exp == "scan-left" ? "dark" : "bright");
seeker.Step(exp => exp == "scan-left" ? "dark" : "bright");

TreeNode tree = IdealEmbodiedDecisionTreeAdapter.Export(seeker);
string html = tree.ToHtmlTree();
```

## RL Test Commands

- `dotnet test Haden.ConsoleTests/Haden.ConsoleTests.csproj --logger "console;verbosity=detailed"`

## Linux Migration Review Library

- Project: `Haden.LinuxMigrationReview/Haden.LinuxMigrationReview.csproj`
- Purpose: keep extracted platform-neutral logic from recently added Windows-only paths in a Linux-reviewable library.
- Current extracted logic: adaptive seek-cycle state update (`AdaptiveSeekCycle`).

## Linux NXT SDK

- Project: `Haden.NxtSDK/Haden.NxtSDK.csproj`
- Scope: headless NXT serial protocol client for connect, sensor input reads, motor output commands, and disconnect.

## Linux Hardware Smoke

- Project: `Haden.HardwareSmoke/Haden.HardwareSmoke.csproj`
- Default command: `dotnet run --project Haden.HardwareSmoke/Haden.HardwareSmoke.csproj`
- Auto-connect source order: command arg path -> `HADEN_NXT_PORT` -> `/dev/rfcomm0`
- Detailed pairing/connectivity guide: `NXTRobot Setup.md`

## Linux Robot Behavior

- Project: `Haden.RobotBehavior/Haden.RobotBehavior.csproj`
- Scope: extracted non-UI light-seeking behavior logic from legacy manual/simulator flows (turn-choice and sensor-difference decision routines).

## Linux Simulation

- Project: `Haden.Simulation/Haden.Simulation.csproj`
- Scope: headless simulation whirl engine for light-seeking state updates using `Haden.Library.WhirlEngine`.

## Errata

```
Robot17 - 00:16:53:17:9B:47

sdptool browse 00:16:53:17:9B:47 | grep -E "Service Name|Channel|Serial Port" -A2
```