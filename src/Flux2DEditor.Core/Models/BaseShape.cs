using Flux2DEditor.Core.Geometry;
using Flux2DEditor.Core.Rendering;
using System.Numerics;

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
        public abstract BoundingBox Bounds { get; }

        #endregion

        #region Editing API (to be implemented by derived shapes)

        public abstract bool HitTest(Vector2 point);
        public abstract void Move(Vector2 offset);
        public abstract bool HitTestHandle(Vector2 point, out int handleIndex);
        public abstract void ResizeFromHandle(int handleIndex, Vector2 newPoint);
        public virtual void SetBounds(BoundingBox newBounds)
        {
            var field = GetType().GetField("_bounds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(this, newBounds);
            }
        }
        public abstract IShape Clone();

        public abstract void Draw(IRenderContext context, RenderStyles styles);

        protected void DrawHandles(IRenderContext ctx, RenderStyles styles, BoundingBox[] handles)
        {
            foreach (var h in handles)
            {
                ctx.FillRectangle(h, styles.HandleFill);
                ctx.DrawRectangle(h, styles.HandleStroke);
            }
        }

        #endregion
    }
}
