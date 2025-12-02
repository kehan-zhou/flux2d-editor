using Flux2DEditor.Core.Geometry;
using Flux2DEditor.Core.Rendering;
using System.Numerics;

namespace Flux2DEditor.Core.Models
{
    /// <summary>
    /// Axis-aligned rectangle shape with four handles (corners)
    /// Handle indices: 0=TopLeft, 1=TopRight, 2=BottomRight, 3=BottomLeft
    /// </summary>
    public class RectangleShape : BaseShape
    {
        #region Fields

        private BoundingBox _bounds;

        #endregion

        #region Properties

        public override BoundingBox Bounds => _bounds;

        #endregion

        #region Constructors

        public RectangleShape(BoundingBox bounds)
        {
            _bounds = bounds;
        }

        #endregion

        #region Editing API

        public override bool HitTest(Vector2 point)
        {
            return _bounds.Contains(point);
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
            var min = _bounds.Min;
            var max = _bounds.Max;

            switch (handleIndex)
            {
                case 0: //Top-left
                    min = newPoint;
                    break;
                
                case 1: //Top-right
                    min = new Vector2(min.X, newPoint.Y);
                    max = new Vector2(newPoint.X, max.Y);
                    break;
                
                case 2: //Bottom-right
                    max = newPoint;
                    break;
                
                case 3: //Bottom-left
                    min = new Vector2(newPoint.X, min.Y);
                    max = new Vector2(max.X, newPoint.Y);
                    break;
                
                default:
                    return; // Invalid handle index
            }

            _bounds = new BoundingBox(min, max);
        }

        public override void SetBounds(BoundingBox newBounds)
        {
            _bounds = newBounds;
        }

        public override IShape Clone()
        {
            return new RectangleShape(_bounds)
            {
                IsSelected = false
            };
        }

        #endregion

        public override void Draw(IRenderContext context, RenderStyles styles)
        {
            if (styles.Fill.Color.W > 0)
            {
                context.FillRectangle(_bounds, styles.Fill);
            }

            var stroke = IsSelected ? styles.SelectedStroke : styles.NormalStroke;
            context.DrawRectangle(_bounds, stroke);

            if (IsSelected)
            {
                DrawHandles(context, styles, GetHandleRects());
            }
        }

        public BoundingBox[] GetHandleRects()
        {
            const float handleSize = 10f;
            
            return new[]
            {
                new BoundingBox(_bounds.Min.X - handleSize / 2f, _bounds.Min.Y - handleSize / 2f, handleSize, handleSize), // Top-left
                new BoundingBox(_bounds.Max.X - handleSize / 2f, _bounds.Min.Y - handleSize / 2f, handleSize, handleSize), // Top-right
                new BoundingBox(_bounds.Max.X - handleSize / 2f, _bounds.Max.Y - handleSize / 2f, handleSize, handleSize), // Bottom-right
                new BoundingBox(_bounds.Min.X - handleSize / 2f, _bounds.Max.Y - handleSize / 2f, handleSize, handleSize) // Bottom-left
            };
        }
    }
}
