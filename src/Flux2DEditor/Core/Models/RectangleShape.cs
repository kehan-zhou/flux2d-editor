namespace Flux2DEditor.Core.Models
{
    /// <summary>
    /// Axis-aligned rectangle shape with four handles (corners)
    /// Handle indices: 0=TopLeft, 1=TopRight, 2=BottomRight, 3=BottomLeft
    /// </summary>
    public class RectangleShape : BaseShape
    {
        #region Fields

        private RectangleF _bounds;

        #endregion

        #region Properties

        public override RectangleF Bounds
        {
            get => _bounds;
        }

        #endregion

        #region Constructors

        public RectangleShape(RectangleF bounds)
        {
            _bounds = bounds;
        }

        #endregion

        #region Editing API

        public override bool HitTest(PointF point)
        {
            return _bounds.Contains(point);
        }

        public override void Move(PointF offset)
        {
            _bounds = new RectangleF(
                _bounds.X + offset.X,
                _bounds.Y + offset.Y,
                _bounds.Width,
                _bounds.Height
            );
        }

        public override bool HitTestHandle(PointF point, out int handleIndex)
        {
            var handles = GetHandleRects();
            for (int i = 0; i < handles.Length; i++)
            {
                if (handles[i].Contains(point))
                {
                    handleIndex = i;
                    return true;
                }
            }

            handleIndex = -1;
            return false;
        }

        public override void ResizeFromHandle(int handleIndex, PointF newPoint)
        {
            var topLeft = new PointF(_bounds.Left, _bounds.Top);
            var bottomRight = new PointF(_bounds.Right, _bounds.Bottom);

            switch (handleIndex)
            {
                case 0: //TopLeft
                    topLeft = newPoint;
                    break;
                case 1: //TopRight
                    topLeft = new PointF(_bounds.Left, newPoint.Y);
                    bottomRight = new PointF(newPoint.X, _bounds.Bottom);
                    break;
                case 2: //BottomRight
                    bottomRight = newPoint;
                    break;
                case 3: //BottomLeft
                    topLeft = new PointF(newPoint.X, _bounds.Top);
                    bottomRight = new PointF(_bounds.Right, newPoint.Y);
                    break;
                default:
                    return; // Invalid handle index
            }

            // Normalize rectangle
            var x = Math.Min(topLeft.X, bottomRight.X);
            var y = Math.Min(topLeft.Y, bottomRight.Y);
            var width = Math.Abs(bottomRight.X - topLeft.X);
            var height = Math.Abs(bottomRight.Y - topLeft.Y);
            _bounds = new RectangleF(x, y, width, height);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns the handle rectangles at the four corners.
        /// </summary>
        /// <returns>Array of RectangleF for corners.</returns>
        public RectangleF[] GetHandleRects()
        {
            const float handleSize = 8f;
            return
            [
                new RectangleF(_bounds.Left - handleSize / 2f, _bounds.Top - handleSize / 2f, handleSize, handleSize), // Top-left
                new RectangleF(_bounds.Right - handleSize / 2f, _bounds.Top - handleSize / 2f, handleSize, handleSize), // Top-right
                new RectangleF(_bounds.Right - handleSize / 2f, _bounds.Bottom - handleSize / 2f, handleSize, handleSize), // Bottom-right
                new RectangleF(_bounds.Left - handleSize / 2f, _bounds.Bottom - handleSize / 2f, handleSize, handleSize) // Bottom-left
            ];
        }

        #endregion
    }
}
