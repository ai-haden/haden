using System;

namespace Haden.RobotBehavior
{
    public sealed class BoredomDirectionPolicy
    {
        private readonly int _flatDeltaThreshold;
        private readonly int _flatLimit;
        private readonly int _uncertainLimit;
        private readonly int _peakUnsureMargin;
        private readonly int _peakConfirmTicks;
        private readonly int _stuckSameDirectionLimit;
        private readonly int _uncertainAlternateSteps;

        public BoredomDirectionPolicy(
            int flatDeltaThreshold = 1,
            int flatLimit = 6,
            int uncertainLimit = 4,
            int peakUnsureMargin = 2,
            int peakConfirmTicks = 4,
            int stuckSameDirectionLimit = 5,
            int uncertainAlternateSteps = 3)
        {
            if (flatDeltaThreshold < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(flatDeltaThreshold), "Flat delta threshold must be non-negative.");
            }

            if (flatLimit <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(flatLimit), "Flat limit must be positive.");
            }

            if (uncertainLimit <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(uncertainLimit), "Uncertain limit must be positive.");
            }

            if (peakUnsureMargin < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(peakUnsureMargin), "Peak unsure margin must be non-negative.");
            }

            if (peakConfirmTicks <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(peakConfirmTicks), "Peak confirm ticks must be positive.");
            }

            if (stuckSameDirectionLimit <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(stuckSameDirectionLimit), "Stuck same-direction limit must be positive.");
            }

            if (uncertainAlternateSteps <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(uncertainAlternateSteps), "Uncertain alternate steps must be positive.");
            }

            _flatDeltaThreshold = flatDeltaThreshold;
            _flatLimit = flatLimit;
            _uncertainLimit = uncertainLimit;
            _peakUnsureMargin = peakUnsureMargin;
            _peakConfirmTicks = peakConfirmTicks;
            _stuckSameDirectionLimit = stuckSameDirectionLimit;
            _uncertainAlternateSteps = uncertainAlternateSteps;
        }

        public BoredomDirectionStep Next(
            BoredomDirectionState state,
            bool hasDirectionalEvidence,
            int evidenceDirection,
            int delta,
            int smoothedLight,
            int peakLightValue,
            int peakStableTicks)
        {
            int biasDirection = state.BiasDirection == 0 ? 1 : Math.Sign(state.BiasDirection);
            bool nearPeakConfirmed =
                peakLightValue != int.MinValue &&
                (peakLightValue - smoothedLight) <= _peakUnsureMargin &&
                peakStableTicks >= _peakConfirmTicks;

            int flatCount = state.FlatCount;
            int uncertainCount = state.UncertainCount;
            int uncertainExploreCount = state.UncertainExploreCount;
            int sameDirectionStuckCount = state.SameDirectionStuckCount;
            int lastActionDirection = state.LastActionDirection;

            if (hasDirectionalEvidence)
            {
                flatCount = 0;
                uncertainCount = 0;
                uncertainExploreCount = 0;
                if (evidenceDirection != 0)
                {
                    biasDirection = Math.Sign(evidenceDirection);
                }
            }
            else
            {
                uncertainCount++;
                if (!nearPeakConfirmed && Math.Abs(delta) <= _flatDeltaThreshold)
                {
                    flatCount++;
                }
                else
                {
                    flatCount = 0;
                }
            }

            bool boredomTriggered =
                !hasDirectionalEvidence &&
                !nearPeakConfirmed &&
                flatCount >= _flatLimit &&
                uncertainCount >= _uncertainLimit;

            int actionDirection = 0;
            bool exploring = false;
            bool forcedFlip = false;
            if (hasDirectionalEvidence)
            {
                actionDirection = evidenceDirection;
            }
            else if (!nearPeakConfirmed && uncertainCount >= _uncertainLimit)
            {
                exploring = true;
                uncertainExploreCount++;
                if (boredomTriggered)
                {
                    biasDirection = -biasDirection;
                    flatCount = 0;
                    uncertainCount = 0;
                    uncertainExploreCount = 0;
                }
                else if (uncertainExploreCount % _uncertainAlternateSteps == 0)
                {
                    biasDirection = -biasDirection;
                }

                actionDirection = biasDirection;
            }
            else
            {
                uncertainExploreCount = 0;
            }

            if (actionDirection != 0)
            {
                if (!nearPeakConfirmed && actionDirection == lastActionDirection && delta <= 0)
                {
                    sameDirectionStuckCount++;
                }
                else
                {
                    sameDirectionStuckCount = 0;
                }

                if (!nearPeakConfirmed && sameDirectionStuckCount >= _stuckSameDirectionLimit)
                {
                    actionDirection = -actionDirection;
                    biasDirection = actionDirection;
                    sameDirectionStuckCount = 0;
                    uncertainExploreCount = 0;
                    forcedFlip = true;
                    exploring = true;
                }

                lastActionDirection = actionDirection;
            }
            else
            {
                sameDirectionStuckCount = 0;
            }

            return new BoredomDirectionStep(
                actionDirection,
                boredomTriggered,
                exploring,
                forcedFlip,
                nearPeakConfirmed,
                new BoredomDirectionState(
                    biasDirection,
                    flatCount,
                    uncertainCount,
                    lastActionDirection,
                    sameDirectionStuckCount,
                    uncertainExploreCount));
        }
    }

    public readonly struct BoredomDirectionState
    {
        public BoredomDirectionState(
            int biasDirection,
            int flatCount,
            int uncertainCount,
            int lastActionDirection,
            int sameDirectionStuckCount,
            int uncertainExploreCount)
        {
            BiasDirection = biasDirection == 0 ? 1 : Math.Sign(biasDirection);
            FlatCount = Math.Max(0, flatCount);
            UncertainCount = Math.Max(0, uncertainCount);
            LastActionDirection = lastActionDirection == 0 ? 0 : Math.Sign(lastActionDirection);
            SameDirectionStuckCount = Math.Max(0, sameDirectionStuckCount);
            UncertainExploreCount = Math.Max(0, uncertainExploreCount);
        }

        public int BiasDirection { get; }
        public int FlatCount { get; }
        public int UncertainCount { get; }
        public int LastActionDirection { get; }
        public int SameDirectionStuckCount { get; }
        public int UncertainExploreCount { get; }
    }

    public readonly struct BoredomDirectionStep
    {
        public BoredomDirectionStep(
            int actionDirection,
            bool boredomTriggered,
            bool exploring,
            bool forcedFlip,
            bool nearPeakConfirmed,
            BoredomDirectionState state)
        {
            ActionDirection = actionDirection == 0 ? 0 : Math.Sign(actionDirection);
            BoredomTriggered = boredomTriggered;
            Exploring = exploring;
            ForcedFlip = forcedFlip;
            NearPeakConfirmed = nearPeakConfirmed;
            State = state;
        }

        public int ActionDirection { get; }
        public bool BoredomTriggered { get; }
        public bool Exploring { get; }
        public bool ForcedFlip { get; }
        public bool NearPeakConfirmed { get; }
        public BoredomDirectionState State { get; }
    }
}
