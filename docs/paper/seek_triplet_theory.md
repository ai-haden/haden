# Seek-Triplet Theory (Sense -> Decide -> Move)

This note documents the theory represented by the live `--seek-max-light` code path in `Haden.HardwareSmoke/Program.cs`.

## Core claim

The robot should not move blindly.
It should first seek evidence in the environment (scan), then decide a direction, then move.
This follows your framing that ordinary animals typically sample the environment before locomotion toward a goal.

## Behavioral loop used in code

Each iteration executes a strict sequence:

1. Sense:
   - Probe three light values using motor A scan-head movement: left, center, right.
   - Keep the head returned to center after each probe sequence.
2. Decide:
   - Compute contrast from `(left, center, right)` via `LightTripletDecisionPolicy`.
   - Require directional evidence and minimum confidence before allowing movement.
3. Move:
   - If evidence passes threshold: steer wheel motors B/C toward brighter side.
   - If evidence is weak/flat: hold wheel movement (`actionMode=hold`) and continue probing.
4. Learn:
   - Persist state/action/reward into SQLite (`experiment_session`, `experiment_step`, `rl_point`, `rl_scorecard`).
5. Terminate episode:
   - A full genuine RL episode ends on front bump-sensor trigger, not an arbitrary timer.
   - Iteration cap remains only as a safety fallback.

## Why adaptive probing exists

A flat triplet can mean either:
- no strong light gradient at current pose, or
- insufficient scan-head travel/amplitude to observe contrast.

The implementation escalates probe amplitude/power after repeated flat observations, then retries sensing before locomotion.
This preserves the sense-first principle while improving observability.

## Extrapolation for paper analysis

This design supports reporting behavior at three layers:

- Perception quality:
  - `L`, `sensorRaw/sensorSmooth`, `R`, `probeConfidence`
- Decision quality:
  - `actionMode` (`seek` vs `hold`), chosen steering direction, confidence threshold checks
- Outcome quality:
  - reward trajectory, bump-completion success, scorecard confidence trends across sessions

From this, papers can distinguish:
- intelligent seeking (evidence-gated steering),
- uncertainty handling (hold + adaptive probe escalation),
- and true task completion (bump-defined terminal success).
