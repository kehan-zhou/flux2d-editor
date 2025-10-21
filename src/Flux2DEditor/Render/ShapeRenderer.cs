using Flux2DEditor.Core.Models;

namespace Flux2DEditor.Render
{
    /// <summary>
    /// Centralized renderer for shapes using GDI+.
    /// This keeps drawing code out of the model classes.
    /// </summary>
    public class ShapeRenderer : IShapeRenderer
    {
        #region Public Methods

        public void Render(IShape shape, Graphics g)
        {
            switch (shape)
            {
                case CircleShape circle:
                    DrawCircle(circle, g);
                    break;
                case RectangleShape rect:
                    DrawRectangle(rect, g);
                    break;
                default:
                    DrawUnknown(shape, g);
                    break;
            }
        }

        #endregion

        #region Private Drawing Helpers

        private void DrawCircle(CircleShape circle, Graphics g)
        {
            using var pen = new Pen(circle.IsSelected ? Color.Blue : Color.Black, 2f);
            g.DrawEllipse(pen, circle.Bounds);

            if (circle.IsSelected)
            {
                DrawHandles(circle.GetHandleRects(), g);
            }
        }

        private void DrawRectangle(RectangleShape rect, Graphics g)
        {
            using var pen = new Pen(rect.IsSelected ? Color.Blue : Color.Black, 2f);
            g.DrawRectangle(pen, rect.Bounds.X, rect.Bounds.Y, rect.Bounds.Width, rect.Bounds.Height);

            if (rect.IsSelected)
            {
                DrawHandles(rect.GetHandleRects(), g);
            }
        }

        private void DrawHandles(RectangleF[] handles, Graphics g)
        {
            using var brush = new SolidBrush(Color.White);
            using var pen = new Pen(Color.Black, 1f);
            foreach (var handle in handles)
            {
                g.FillRectangle(brush, handle);
                g.DrawRectangle(pen, handle.X, handle.Y, handle.Width, handle.Height);
            }
        }

        private void DrawUnknown(IShape shape, Graphics g)
        {
            using var pen = new Pen(Color.Magenta, 1f);
            g.DrawRectangle(pen, shape.Bounds.X, shape.Bounds.Y, shape.Bounds.Width, shape.Bounds.Height);
        }

        #endregion
    }
}