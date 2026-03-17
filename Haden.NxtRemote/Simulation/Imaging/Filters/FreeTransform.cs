using System;
using System.Drawing;
using Haden.NXTRemote.Simulation.Drawing3D;

namespace Haden.NXTRemote.Simulation.Imaging.Filters
{
    public class FreeTransform
    {
        PointF[] _vertex = new PointF[4];
        Vector _ab, _bc, _cd, _da;
        Rectangle _rect;
        readonly ImageData _srcCb = new ImageData();
        int _srcW;
        int _srcH;

        public Bitmap Bitmap
        {
            set
            {
                try
                {
                    _srcCb.FromBitmap(value);
                    _srcH = value.Height;
                    _srcW = value.Width;
                }
                catch
                {
                    _srcW = 0; _srcH = 0;
                }
            }
            get
            {
                return GetTransformedBitmap();
            }
        }

        public Point ImageLocation
        {
            set { _rect.Location = value; }
            get { return _rect.Location; }
        }

        bool _isBilinear;
        public bool IsBilinearInterpolation
        {
            set { _isBilinear = value; }
            get { return _isBilinear; }
        }

        public int ImageWidth
        {
            get { return _rect.Width; }
        }

        public int ImageHeight
        {
            get { return _rect.Height; }
        }

        public PointF VertexLeftTop
        {
            set { _vertex[0] = value; SetVertex(); }
            get { return _vertex[0]; }
        }

        public PointF VertexTopRight
        {
            set { _vertex[1] = value; SetVertex(); }
            get { return _vertex[1]; }
        }

        public PointF VertexRightBottom
        {
            set { _vertex[2] = value; SetVertex(); }
            get { return _vertex[2]; }
        }

        public PointF VertexBottomLeft
        {
            set { _vertex[3] = value; SetVertex(); }
            get { return _vertex[3]; }
        }

        public PointF[] FourCorners
        {
            set { _vertex = value; SetVertex(); }
            get { return _vertex; }
        }

        private void SetVertex()
        {
            float xmin = float.MaxValue;
            float ymin = float.MaxValue;
            float xmax = float.MinValue;
            float ymax = float.MinValue;

            for (int i = 0; i < 4; i++)
            {
                xmax = Math.Max(xmax, _vertex[i].X);
                ymax = Math.Max(ymax, _vertex[i].Y);
                xmin = Math.Min(xmin, _vertex[i].X);
                ymin = Math.Min(ymin, _vertex[i].Y);
            }

            _rect = new Rectangle((int)xmin, (int)ymin, (int)(xmax - xmin+2), (int)(ymax - ymin+2));

            _ab = new Vector(_vertex[0], _vertex[1]);
            _bc = new Vector(_vertex[1], _vertex[2]);
            _cd = new Vector(_vertex[2], _vertex[3]);
            _da = new Vector(_vertex[3], _vertex[0]);

            // get unit vector
            _ab /= _ab.Magnitude;
            _bc /= _bc.Magnitude;
            _cd /= _cd.Magnitude;
            _da /= _da.Magnitude;
        }

        private bool IsOnPlaneAbcd(PointF pt) //  including point on border
        {
            if (!Vector.IsCCW(pt, _vertex[0], _vertex[1]))
            {
                if (!Vector.IsCCW(pt, _vertex[1], _vertex[2]))
                {
                    if (!Vector.IsCCW(pt, _vertex[2], _vertex[3]))
                    {
                        if (!Vector.IsCCW(pt, _vertex[3], _vertex[0]))
                            return true;
                    }
                }
            }
            return false;
        }

        private Bitmap GetTransformedBitmap()
        {
            if (_srcH == 0 || _srcW == 0) return null;

            ImageData destCb = new ImageData();
            destCb.A = new byte[_rect.Width, _rect.Height];
            destCb.B = new byte[_rect.Width, _rect.Height];
            destCb.G = new byte[_rect.Width, _rect.Height];
            destCb.R = new byte[_rect.Width, _rect.Height];


            PointF ptInPlane = new PointF();

            for (int y = 0; y < _rect.Height; y++)
            {
                for (int x = 0; x < _rect.Width; x++)
                {
                    Point srcPt = new Point(x, y);
                    srcPt.Offset(_rect.Location);

                    if (IsOnPlaneAbcd(srcPt))
                    {
                        double dab = Math.Abs((new Vector(_vertex[0], srcPt)).CrossProduct(_ab));
                        double dbc = Math.Abs((new Vector(_vertex[1], srcPt)).CrossProduct(_bc));
                        double dcd = Math.Abs((new Vector(_vertex[2], srcPt)).CrossProduct(_cd));
                        double dda = Math.Abs((new Vector(_vertex[3], srcPt)).CrossProduct(_da));
                        ptInPlane.X = (float)(_srcW * (dda / (dda + dbc)));
                        ptInPlane.Y = (float)(_srcH * (dab / (dab + dcd)));

                        int x1 = (int)ptInPlane.X;
                        int y1 = (int)ptInPlane.Y;

                        if (x1 >= 0 && x1 < _srcW && y1 >= 0 && y1 < _srcH)
                        {
                            if (_isBilinear)
                            {
                                int x2 = (x1 == _srcW - 1) ? x1 : x1 + 1;
                                int y2 = (y1 == _srcH - 1) ? y1 : y1 + 1;

                                float dx1 = ptInPlane.X - x1;
                                if (dx1 < 0) dx1 = 0;
                                dx1 = 1f - dx1;
                                float dx2 = 1f - dx1;
                                float dy1 = ptInPlane.Y - y1;
                                if (dy1 < 0) dy1 = 0;
                                dy1 = 1f - dy1;
                                float dy2 = 1f - dy1;

                                float dx1Y1 = dx1 * dy1;
                                float dx1Y2 = dx1 * dy2;
                                float dx2Y1 = dx2 * dy1;
                                float dx2Y2 = dx2 * dy2;

                                float nbyte = _srcCb.A[x1, y1] * dx1Y1 + _srcCb.A[x2, y1] * dx2Y1 + _srcCb.A[x1, y2] * dx1Y2 + _srcCb.A[x2, y2] * dx2Y2;
                                destCb.A[x, y] = (byte)nbyte;
                                nbyte = _srcCb.B[x1, y1] * dx1Y1 + _srcCb.B[x2, y1] * dx2Y1 + _srcCb.B[x1, y2] * dx1Y2 + _srcCb.B[x2, y2] * dx2Y2;
                                destCb.B[x, y] = (byte)nbyte;
                                nbyte = _srcCb.G[x1, y1] * dx1Y1 + _srcCb.G[x2, y1] * dx2Y1 + _srcCb.G[x1, y2] * dx1Y2 + _srcCb.G[x2, y2] * dx2Y2;
                                destCb.G[x, y] = (byte)nbyte;
                                nbyte = _srcCb.R[x1, y1] * dx1Y1 + _srcCb.R[x2, y1] * dx2Y1 + _srcCb.R[x1, y2] * dx1Y2 + _srcCb.R[x2, y2] * dx2Y2;
                                destCb.R[x, y] = (byte)nbyte;
                            }
                            else
                            {
                                destCb.A[x, y] = _srcCb.A[x1, y1];
                                destCb.B[x, y] = _srcCb.B[x1, y1];
                                destCb.G[x, y] = _srcCb.G[x1, y1];
                                destCb.R[x, y] = _srcCb.R[x1, y1];
                            }
                        }
                    }
                }
            }
            return destCb.ToBitmap();
        }
    }
}
