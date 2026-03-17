using System;

namespace Haden.NXTRemote.Simulation.Plotting3D
{
    public class Dominoes
    {
        private readonly float _distanceBetweenDominoes;
        private readonly float _angleBetweenDominoes;
        private const int FramesBeforeConnection = 5;
        private readonly Domino[] _dominoArray;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dominoCount"></param>
        /// <param name="dominoWidth"></param>
        /// <param name="dominoHeight"></param>
        /// <param name="dominoDepth"></param>
        /// <param name="distanceBetweenDominoes"></param>
        public Dominoes(int dominoCount, float dominoWidth, float dominoHeight, float dominoDepth, float distanceBetweenDominoes)
        {
            _dominoArray = new Domino[dominoCount];

            for (int i = 0; i < _dominoArray.Length; i++)
            {
                _dominoArray[i] = new Domino(dominoWidth, dominoHeight, dominoDepth);
            }

            _distanceBetweenDominoes = distanceBetweenDominoes;
            // Store the angle of a falling domino when it first connects with the domino after it. Store it in degrees.
            _angleBetweenDominoes = (float)(Math.Acos(distanceBetweenDominoes / dominoHeight) * 180 / Math.PI);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public void Render(Plotter3D p)
        {
            foreach (Domino d in _dominoArray)
            {
                d.Render(p);

                p.IsPenDown = false;
                p.Forward(_distanceBetweenDominoes);
                p.IsPenDown = true;
            }
        }
        public delegate void PositionChangedEventHandler(object sender, EventArgs e);
        public event PositionChangedEventHandler PositionChanged;
        /// <summary>
        /// 
        /// </summary>
        public void FallOver()
        {
            for (int lastFallingDomino = 0; lastFallingDomino < _dominoArray.Length; lastFallingDomino++)
            {
                for (int frame = 0; frame < FramesBeforeConnection; frame++)
                {
                    _dominoArray[lastFallingDomino].FallAngle = 90 - ((90 - _angleBetweenDominoes) * frame / FramesBeforeConnection);

                    for (int parentDomino = lastFallingDomino - 1; parentDomino >= 0; parentDomino--)
                    {
                        _dominoArray[parentDomino].FallAngle = (float)MathematicsUtilities.FindInnerAngle(_dominoArray[parentDomino + 1].FallAngle, _dominoArray[0].Height, _dominoArray[0].Depth, _distanceBetweenDominoes);
                    }

                    PositionChanged?.Invoke(this, EventArgs.Empty);
                }
            }

            Domino lastDomino = _dominoArray[_dominoArray.Length - 1];
            float fallAngle = lastDomino.FallAngle;
            float distancePerFrame = _angleBetweenDominoes / FramesBeforeConnection;

            while (fallAngle > 0)
            {
                fallAngle -= distancePerFrame;
                lastDomino.FallAngle = Math.Max(fallAngle, 0);

                for (int parentDomino = _dominoArray.Length - 2; parentDomino >= 0; parentDomino--)
                {
                    _dominoArray[parentDomino].FallAngle = (float)MathematicsUtilities.FindInnerAngle(_dominoArray[parentDomino + 1].FallAngle, _dominoArray[0].Height, _dominoArray[0].Depth, _distanceBetweenDominoes);
                }

                PositionChanged?.Invoke(this, EventArgs.Empty);
            }

        }
    }
}
