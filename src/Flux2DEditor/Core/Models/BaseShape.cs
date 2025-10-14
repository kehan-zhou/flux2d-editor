using Flux2DEditor.Core.Interfaces;

namespace Flux2DEditor.Core.Models
{
    /// <summary>
    /// Base implementation for shapes: contains selection state and base helpers.
    /// Concrete shapes should implement geometry and handle logic.
    /// </summary>
    public abstract class BaseShape : IShape
    {
        #region Properties

        /// <summary>
        /// Gets or sets whether the shape is selected.
        /// </summary>
        public bool IsSelected { get; set; } = false;

        /// <summary>
        /// Gets the bounding box in world coordinates.
        /// </summary>
        public abstract RectangleF Bounds { get; }

        #endregion

        #region Editing API (to be implemented by derived shapes)

        public abstract bool HitTest(PointF point);
        public abstract void Move(PointF offset);
        public abstract bool HitTestHandle(PointF point, out int handleIndex);
        public abstract void ResizeFromHandle(int handleIndex, PointF newPoint);

        #endregion
    }
}
