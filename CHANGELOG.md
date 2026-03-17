# Changelog

All notable fixes in this repository should be documented here.

## [Unreleased]

### Added
- Established changelog workflow for tracking code fixes over time.

### Fixed
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

## Entry Template For Future Fixes

Use this format when adding new fixes:

```md
## [Unreleased]

### Fixed
- `relative/path/to/file.cs`
  - Brief description of the bug.
  - Brief description of the behavior change.
```
