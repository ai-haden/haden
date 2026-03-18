# Changelog

All notable fixes in this repository should be documented here.

## [Unreleased]

### Added
- Established changelog workflow for tracking code fixes over time.

### Fixed
- `Haden.ConsoleTests/Haden.ConsoleTests.csproj`
- `Haden.ConsoleTests/Branch12ConsoleTests.cs`
- `Haden.ConsoleTests/RewardConsoleTests.cs`
- `Haden.ConsoleTests/AdaptiveSeekCycleTests.cs`
- `Haden.ConsoleTests/NxtSdkProtocolTests.cs`
- `Haden.LinuxMigrationReview/Haden.LinuxMigrationReview.csproj`
- `Haden.LinuxMigrationReview/AdaptiveSeekCycle.cs`
- `Haden.NxtSDK/Haden.NxtSDK.csproj`
- `Haden.NxtSDK/Transport.cs`
- `Haden.NxtSDK/NxtProtocolTypes.cs`
- `Haden.NxtSDK/NxtExceptions.cs`
- `Haden.NxtSDK/NxtPacketCodec.cs`
- `Haden.NxtSDK/NxtBrickClient.cs`
- `Haden.HardwareSmoke/Haden.HardwareSmoke.csproj`
- `Haden.HardwareSmoke/Program.cs`
- `Haden.RobotBehavior/Haden.RobotBehavior.csproj`
- `Haden.RobotBehavior/LightSeekBehavior.cs`
- `Haden.ConsoleTests/RobotBehaviorTests.cs`
- `Haden.Simulation/Haden.Simulation.csproj`
- `Haden.Simulation/LightSeekSimulationEngine.cs`
- `Haden.ConsoleTests/SimulationWhirlTests.cs`
- `Haden.NxtSharp/`
- `Haden.NxtRemote/`
- `Haden.Tests/`
- `Haden.Autonomy.sln`
- `README.md`
- `NEXT_SESSION_PROMPT.md`
- `Haden.Tests/HardwareLightSeekingConsoleTests.cs`
  - Bug: recently added light-seeking logic was only present in a Windows-only hardware test path and not isolated for Linux review.
  - Behavior change: extracted adaptive seek-cycle behavior into a new `net9.0` Linux review library, added Linux console tests for it, removed the Windows-only branch-added test file, removed Windows-only projects from the active solution, introduced a new headless `Haden.NxtSDK` Linux project for NXT serial protocol operations, migrated remaining legacy communicator methods (brick naming, mailbox messaging, low-speed/I2C), added `ConnectWithRetry` for project-level auto-connect, added a runnable `Haden.HardwareSmoke` console project that auto-connects to `/dev/rfcomm0` (or `HADEN_NXT_PORT`) and performs connect/keepalive/battery/disconnect, extracted non-UI light-seeking decision logic from historical manual/simulator forms into `Haden.RobotBehavior`, added a headless `Haden.Simulation` engine that reuses `Haden.Library.WhirlEngine` for whirl-state progression, added regression tests, removed old Windows-only project directories, and trimmed docs/prompts to Linux-only workflow guidance.
- `global.json`
  - Bug: no SDK pin existed, so local `dotnet` selected `9.0.312`, which failed restore in this environment before tests could execute.
  - Behavior change: pinned SDK to `9.0.100` with roll-forward disabled to force a known-installed SDK for deterministic local build/test runs.
- `Haden.Tests/HardwareSmokeTests.cs`
  - Bug: no single deterministic hardware smoke test existed for the required NXT Bluetooth validation path (connect, sensor read loop, motor command, clean disconnect).
  - Behavior change: added a dedicated Windows-only, COM-gated smoke test that requires explicit `HADEN_NXT_COM_PORT` and performs connect/read/move/disconnect in one flow.
- `Haden.Tests/SanityTests.cs`
  - Bug: legacy manual/hardware tests were part of standard discovery and could interfere with default simulation-focused test runs.
  - Behavior change: marked `SanityTests` as explicit hardware/manual integration tests with a hardware category so they are isolated from normal test execution.
- `Haden.NxtSharp/Sensors/NxtSensor.cs`
  - Prevented first-poll null reference by treating `LastResult == null` as a value-change boundary.
- `Haden.NxtSharp/Brick/NxtBrick.cs`
  - Replaced `Thread.Abort()`-based poll shutdown with a cooperative stop flag and thread join.
  - Added polling-loop shutdown checks to improve disconnect stability.
- `Haden.NxtRemote/Forms/HadenManualControl.cs`
  - Corrected iteration branching from an always-true condition to `Iteration > 1`.
  - Removed recursive `Compare()` self-calls that could lead to uncontrolled loops.
  - Updated state progression by assigning `CurrentValue = Now` after compare pass.
  - Initialized COM port selection from detected serial ports (fallback to `COM7`) and propagated selection to `nxtBrick.ComPortName`.
- `Haden.Library/Haden.Library.csproj`
- `Haden.NxtSharp/Haden.NxtSharp.csproj`
- `Haden.NxtRemote/Haden.NxtRemote.csproj`
- `Haden.Tests/Haden.Tests.csproj`
  - Bug: projects used legacy non-SDK .NET Framework 4.8 format, which blocked a direct .NET 9 SDK build workflow.
  - Behavior change: all projects now use SDK-style project files targeting .NET 9 (`net9.0` / `net9.0-windows`) with updated test package references.
- `Haden.NxtSharp/Sensors/NxtSensor.cs`
  - Bug: sensor polling compared a value-type result (`NxtGetInputValues` struct) to `null`, which fails to compile under SDK-style .NET 9 build.
  - Behavior change: polling now uses an explicit first-result flag to trigger `ValueChanged` on first poll and value deltas thereafter.
- `Haden.NxtRemote/Haden.NxtRemote.csproj`
- `Haden.NxtRemote/SpeechLibCompat.cs`
  - Bug: `SpeechLib` COM reference (`Interop.SpeechLib`) was incompatible with the .NET 9 SDK migration path in this solution.
  - Behavior change: speech calls now route through an in-project `SpeechLib` compatibility shim backed by `AeonVoice` (`SpVoice`/`SpeechVoiceSpeakFlags` preserved for existing form code).
- `Haden.NxtRemote/Forms/Experimental/PaperAutonomy.cs`
- `Haden.NxtRemote/Forms/Experimental/PaperAutonomy.Designer.cs`
- `Haden.NxtRemote/Forms/Simulation/SimulatedAutonomy.cs`
- `Haden.NxtRemote/Forms/Simulation/SimulatedAutonomy.Designer.cs`
- `Haden.NxtRemote/Forms/Simulation/SimulatorMockup.cs`
- `Haden.NxtRemote/Forms/HadenManualControl.cs`
  - Bug: legacy WinForms menu APIs (`ContextMenu`, `MenuItem`, and `Control.ContextMenu`) failed compilation under the .NET 9 WinForms target.
  - Behavior change: line-context menu wiring now uses `ContextMenuStrip`, `ToolStripMenuItem`, and `Control.ContextMenuStrip`.
- `Haden.NxtRemote/Haden.NxtRemote.csproj`
- `Haden.NxtRemote/Forms/HadenManualControl.cs`
- `Haden.Tests/SanityTests.cs`
  - Bug: .NET 9 migration introduced build blockers from WinForms analyzer `WFO1000` errors and ambiguous `Logging` type resolution (`Haden.NxtSharp.Logging` vs `Haden.NxtSharp.Utilties.Logging`).
  - Behavior change: `Haden.NxtRemote` now suppresses `WFO1000` during migration, and both affected files now bind `Logging` explicitly to `Haden.NxtSharp.Utilties.Logging`.
- `Haden.NxtRemote/Haden.NxtRemote.csproj`
  - Bug: runtime startup failed because `config/Settings.xml` was not copied to the .NET 9 output folder (`bin/Debug/net9.0-windows/config`).
  - Behavior change: `config/Settings.xml` is now copied to output on build, enabling `HadenManualControl` startup settings load.
- `Haden.NxtRemote/config/Settings.xml`
  - Bug: project-level settings source file was missing, causing copy-to-output to fail at build time.
  - Behavior change: repository now includes the settings source file used for runtime configuration copy.
- `Haden.NxtRemote/Forms/HadenManualControl.cs`
  - Bug: default settings path used `Environment.CurrentDirectory`, which may not match the app output folder under .NET 9 run workflows.
  - Behavior change: settings load now prefers `AppContext.BaseDirectory/config/Settings.xml` with fallback to current directory.
- `Haden.Library/HadenCore.cs`
  - Bug: core settings load also depended on `Environment.CurrentDirectory`, causing startup failure when launched from SDK-style output paths.
  - Behavior change: core settings load now prefers `AppContext.BaseDirectory/config/Settings.xml` with fallback to current directory.
- `Haden.NxtRemote/Forms/HadenManualControl.cs`
  - Bug: placeholder `hold` locals in light-event handlers were assigned but never used, producing compiler warnings.
  - Behavior change: removed unused locals while preserving existing light-event placeholder branches.
- `Haden.NxtRemote/Controls/LineWidthDialog.Designer.cs`
  - Bug: designer field `CancelButton` hid `Form.CancelButton` implicitly, producing member-hiding warnings.
  - Behavior change: made field hiding explicit with `new` to keep existing generated naming without warning noise.
- `Haden.NxtRemote/Forms/HadenManualControl.cs`
  - Bug: whirl timer callback performed UI and state updates from a timer thread, invoked action evaluation multiple times per tick, and did not stop/dispose on disconnect or exit.
  - Behavior change: whirl ticks now marshal to the UI thread, evaluate action state once per cycle, and the timer is now stopped/disposed when restarting, disconnecting, exiting, or closing the form.
- `Haden.Library/WhirlEngine.cs`
- `Haden.NxtRemote/Forms/HadenManualControl.cs`
- `Haden.NxtSharp/RueTheWhirl.cs`
- `Haden.NxtSharp/Haden.NxtSharp.csproj`
- `Haden.NxtRemote/Data/Controller.cs`
  - Bug: whirl state-machine and action logic was spread across UI and `Haden.NxtSharp`, making behavior ownership fragmented.
  - Behavior change: primary whirl state/action logic now lives in `Haden.Library.WhirlEngine`; form code consumes library tick results, and `RueTheWhirl` now acts as a compatibility wrapper over the shared engine.
- `Haden.Tests/WhirlEngineTests.cs`
  - Bug: no focused regression test existed for whirl state transitions and seek-window behavior tied to autonomous light-seeking mission flow.
  - Behavior change: added deterministic unit tests validating transition order, seek-phase signaling, and a mission-style brightest-light acquisition simulation.
- `Haden.NxtSharp/Utilties/Logger.cs`
- `Haden.NxtSharp/Logger.cs`
  - Bug: logging wrote to `logs`/`db` files without ensuring parent directories existed, causing runtime/test failures when output folders were fresh.
  - Behavior change: logging now creates required parent directories automatically before opening output files.
- `Haden.Tests/SanityTests.cs`
  - Bug: hardware integration tests hardcoded `COM7` and failed on machines without that exact Bluetooth serial port mapping.
  - Behavior change: tests now select the first available COM port dynamically and skip with a clear message when no Bluetooth serial COM port is present.
- `Haden.Tests/RewardTests.cs`
  - Bug: NUnit test methods were non-public, causing discovery/execution failures (`Method is not public`).
  - Behavior change: reward test methods are now public and discoverable by the .NET test runner.
- `Haden.Library/Algorithm/IdealReinforcementModel.cs`
- `Haden.Tests/RewardTests.cs`
  - Bug: reward tests were placeholders and did not validate reinforcement-learning behavior for autonomous light-seeking.
  - Behavior change: added an `ideal`-style deterministic tabular RL model and implemented assertions for TD updates plus policy convergence toward the higher-reward (brightest-source) action.
- `Haden.Library/Algorithm/IdealEmbodiedLightSeeker.cs`
- `Haden.Tests/IdealBranch12LightSeekerTests.cs`
- `docs/paper/light_seeker_branch12.tex`
  - Bug: branch-01/branch-02 concepts from `cartheur/ideal` were not explicitly encoded or paper-documented for the Haden light-seeker mission.
  - Behavior change: added an embodied-motivational light-seeker model, executable regression tests for boredom/motivation behavior, and a LaTeX section suitable for academic manuscript integration.
- `Haden.Library/Algorithm/IdealEmbodiedLightSeeker.cs`
- `Haden.Tests/RewardTests.cs`
- `docs/paper/light_seeker_branch12.tex`
  - Bug: branch-01/branch-02 tests revealed a mismatch between expected boredom timing, motivational exploration behavior, and numeric TD test expectations.
  - Behavior change: boredom is now emitted on the threshold cycle, motivation can explore unknown experiments when known outcomes are non-positive, TD expected values were corrected, and the LaTeX paper section now includes explicit statements of academic value and evaluation rationale.
- `Haden.Tests/packages.config`
  - Bug: stale legacy NuGet `packages.config` (targeting `.NET Framework 4.8`) remained after SDK-style migration and no longer matched test project dependency management.
  - Behavior change: removed obsolete `packages.config`; test dependencies remain managed exclusively via `PackageReference` in `Haden.Tests.csproj`.
- `Haden.Library/Algorithm/IdealEmbodiedLightSeeker.cs`
- `Haden.Library/Algorithm/IdealEmbodiedDecisionTreeAdapter.cs`
- `Haden.Tests/IdealEmbodiedDecisionTreeAdapterTests.cs`
- `docs/paper/light_seeker_branch12.tex`
- `README.md`
  - Bug: the new embodied branch-01/branch-02 model had no direct adapter into legacy `TreeNode` explainability output, and this path was not discoverable from the repository entry point.
  - Behavior change: added a decision-tree export adapter for embodied interactions, exposed snapshot accessors on the embodied model, added adapter regression coverage, and documented usage in the main README and LaTeX paper section.
- `Haden.Library/Algorithm/BaseLearner.cs`
- `Haden.Library/Algorithm/StateActionSpace.cs`
- `Haden.Tests/BaseLearnerTests.cs`
  - Bug: `BaseLearner.GetNextAction` returned a character code from a policy string (`Policy[currentState][1]`) instead of a valid action id/index.
  - Behavior change: action selection now resolves and returns the configured action index from the action space, with explicit errors for invalid policy mappings; `StateActionSpace` members are now virtual to support deterministic test stubs.
- `Haden.Library/Learning/FeatureValuePair.cs`
- `Haden.Library/Learning/Query.cs`
- `Haden.Library/Learning/Policy.cs`
- `Haden.Library/SettingsDictionary.cs`
  - Bug: medium-severity reliability issues remained in equality logic, mutable policy state exposure, and XML setting parse conditions.
  - Behavior change: `Equals` implementations now safely handle null/type mismatch, `Policy.StateSpace` is now get-only to reduce accidental replacement, and settings XML checks now use short-circuit `&&`.
- `Haden.Library/Algorithm/BaseLearner.cs`
  - Bug: `FinalizeEpisode` swallowed all exceptions and constructor emitted debug output to console, reducing diagnosability and causing noisy test output.
  - Behavior change: removed debug console output, added input validation, and replaced exception swallowing with explicit deterministic-state validation errors.
- `Haden.Library/SettingsDictionary.cs`
  - Bug: settings-load failures threw generic exceptions without contextual path information, making diagnosis harder.
  - Behavior change: settings load now throws explicit argument validation for blank paths and `FileNotFoundException` with the missing file path.

## Entry Template For Future Fixes

Use this format when adding new fixes:

```md
## [Unreleased]

### Fixed
- `relative/path/to/file.cs`
  - Brief description of the bug.
  - Brief description of the behavior change.
```
