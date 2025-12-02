using Flux2DEditor.Core.Geometry;
using Flux2DEditor.Core.Rendering;
using System.Numerics;

namespace Flux2DEditor.Core.Models
{
    /// <summary>
    /// Simple circle shape represented by bounding square (circle fits inside).
    /// Resize and handle indices: 0=Top, 1=Right, 2=Bottom, 3=Left
    /// </summary>
    public class CircleShape : BaseShape
    {
        #region Fields

        private BoundingBox _bounds;

        #endregion

        #region Properties

        public override BoundingBox Bounds => _bounds;

        #endregion

        #region Constructors

        public CircleShape(BoundingBox bounds)
        {
            _bounds = bounds;
        }

        #endregion

        #region Editing API

        public override bool HitTest(Vector2 point)
        {
            var center = _bounds.Center;
            var radius = _bounds.Width * 0.5f;

            var dx = point.X - center.X;
            var dy = point.Y - center.Y;

            return dx * dx + dy * dy <= radius * radius;
        }

        public override void Move(Vector2 offset)
        {
            _bounds = _bounds.Translated(offset);
        }

        public override bool HitTestHandle(Vector2 point, out int handleIndex)
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

        public override void ResizeFromHandle(int handleIndex, Vector2 newPoint)
        {
            var center = _bounds.Center;

            float newRadius = handleIndex switch
            {
                0 => MathF.Abs(center.Y - newPoint.Y), // Top
                1 => MathF.Abs(newPoint.X - center.X), // Right
                2 => MathF.Abs(newPoint.Y - center.Y), // Bottom
                3 => MathF.Abs(center.X - newPoint.X), // Left
                _ => 0f
            };

            if (newRadius <= 0f) return;

            _bounds = new BoundingBox(
                center.X - newRadius,
                center.Y - newRadius,
                newRadius * 2f,
                newRadius * 2f
            );
        }

        public override void SetBounds(BoundingBox newBounds)
        {
            _bounds = newBounds;
        }

        public override IShape Clone()
        {
            return new CircleShape(_bounds)
            {
                IsSelected = false
            };
        }

        #endregion

        public override void Draw(IRenderContext context, RenderStyles styles)
        {
            float radius = _bounds.Width * 0.5f;
            var center = _bounds.Center;

            if (styles.Fill.Color.W > 0)
            {
                context.FillCircle(center, radius, styles.Fill);
            }

            var stroke = IsSelected ? styles.SelectedStroke : styles.NormalStroke;
            context.DrawCircle(center, radius, stroke);

            if (IsSelected)
            {
                DrawHandles(context, styles, GetHandleRects());
            }
        }

        public BoundingBox[] GetHandleRects()
        {
            const float handleSize = 10f;
            
            var center = _bounds.Center;

            return new[]
            {
                new BoundingBox(center.X - handleSize / 2f, _bounds.Min.Y - handleSize / 2f, handleSize, handleSize), // Top
                new BoundingBox(_bounds.Max.X - handleSize / 2f, center.Y - handleSize / 2f, handleSize, handleSize), // Right
                new BoundingBox(center.X - handleSize / 2f, _bounds.Max.Y - handleSize / 2f, handleSize, handleSize), // Bottom
                new BoundingBox(_bounds.Min.X - handleSize / 2f, center.Y - handleSize / 2f, handleSize, handleSize) // Left
            };
        }
    }
}
