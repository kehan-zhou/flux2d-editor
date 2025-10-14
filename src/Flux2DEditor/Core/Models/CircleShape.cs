namespace Flux2DEditor.Core.Models
{
    /// <summary>
    /// Simple circle shape represented by bounding square (circle fits inside).
    /// Resize and handle indices: 0=Top, 1=Right, 2=Bottom, 3=Left
    /// </summary>
    public class CircleShape : BaseShape
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

        public CircleShape(RectangleF bounds)
        {
            _bounds = bounds;
        }

        #endregion

        #region Editing API

        public override bool HitTest(PointF point)
        {
            var center = new PointF(Bounds.X + Bounds.Width / 2, Bounds.Y + Bounds.Height / 2);
            var radius = Math.Min(Bounds.Width, Bounds.Height) / 2;
            var dx = point.X - center.X;
            var dy = point.Y - center.Y;
            return dx * dx + dy * dy <= radius * radius;
        }

        public override void Move(PointF offset)
        {
            _bounds = new RectangleF(
                Bounds.X + offset.X,
                Bounds.Y + offset.Y,
                Bounds.Width,
                Bounds.Height
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
            var centerX = _bounds.X + _bounds.Width / 2;
            var centerY = _bounds.Y + _bounds.Height / 2;

            float newRadius;

            switch (handleIndex)
            {
                case 0: // Top
                    newRadius = Math.Abs(centerY - newPoint.Y);
                    break;
                case 1: // Right
                    newRadius = Math.Abs(newPoint.X - centerX);
                    break;
                case 2: // Bottom
                    newRadius = Math.Abs(newPoint.Y - centerY);
                    break;
                case 3: // Left
                    newRadius = Math.Abs(centerX - newPoint.X);
                    break;
                default:
                    return; // Invalid handle index
            }

            // Update the bounds to maintain the circular shape
            _bounds = new RectangleF(
                centerX - newRadius,
                centerY - newRadius,
                newRadius * 2,
                newRadius * 2
            );
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns handle rectangles in world coordinates.
        /// </summary>
        /// <returns>Array of handle rectangles (Top, Right, Bottom, Left).</returns>
        public RectangleF[] GetHandleRects()
        {
            const float handleSize = 8f;
            var centerX = _bounds.X + _bounds.Width / 2f;
            var centerY = _bounds.Y + _bounds.Height / 2f;

            return new[]
            {
                new RectangleF(centerX - handleSize / 2f, _bounds.Top - handleSize / 2f, handleSize, handleSize), // Top
                new RectangleF(_bounds.Right - handleSize / 2f, centerY - handleSize / 2f, handleSize, handleSize), // Right
                new RectangleF(centerX - handleSize / 2f, _bounds.Bottom - handleSize / 2f, handleSize, handleSize), // Bottom
                new RectangleF(_bounds.Left - handleSize / 2f, centerY - handleSize / 2f, handleSize, handleSize) // Left
            };
        }

        #endregion
    }
}
