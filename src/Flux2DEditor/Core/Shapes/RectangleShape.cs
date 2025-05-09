using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux2DEditor.Core.Shapes
{
    public class RectangleShape : BaseShape
    {
        public RectangleF Bounds { get; private set; }

        public RectangleShape(RectangleF bounds)
        {
            Bounds = bounds;
        }

        public override void Draw(Graphics g)
        {
            using var pen = new Pen(IsSelected ? Color.Blue : Color.Black, 2);
            g.DrawRectangle(pen, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);

            if (IsSelected)
            {
                foreach (var handle in GetHandles())
                {
                    g.FillRectangle(Brushes.White, handle);
                    g.DrawRectangle(Pens.Black, handle.X, handle.Y, handle.Width, handle.Height);
                }
            }
        }

        public override bool HitTest(PointF point)
        {
            return Bounds.Contains(point);
        }

        public override void Move(PointF offset)
        {
            Bounds = new RectangleF(
                Bounds.X + offset.X,
                Bounds.Y + offset.Y,
                Bounds.Width,
                Bounds.Height
            );
        }

        public override bool HitTestHandle(PointF point, out int handleIndex)
        {
            var handles = GetHandles();
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
            float left = Bounds.Left;
            float top = Bounds.Top;
            float right = Bounds.Right;
            float bottom = Bounds.Bottom;

            switch (handleIndex)
            {
                case 0: // Top-left
                    left = newPoint.X;
                    top = newPoint.Y;
                    break;
                case 1: // Top-right
                    right = newPoint.X;
                    top = newPoint.Y;
                    break;
                case 2: // Bottom-right
                    right = newPoint.X;
                    bottom = newPoint.Y;
                    break;
                case 3: // Bottom-left
                    left = newPoint.X;
                    bottom = newPoint.Y;
                    break;
            }

            // Ensure width and height remain positive
            Bounds = new RectangleF(
                Math.Min(left, right),
                Math.Min(top, bottom),
                Math.Abs(right - left),
                Math.Abs(bottom - top)
            );
        }

        private RectangleF[] GetHandles()
        {
            const float handleSize = 8f;
            return
            [
                new RectangleF(Bounds.Left - handleSize / 2, Bounds.Top - handleSize / 2, handleSize, handleSize), // Top-left
                new RectangleF(Bounds.Right - handleSize / 2, Bounds.Top - handleSize / 2, handleSize, handleSize), // Top-right
                new RectangleF(Bounds.Right - handleSize / 2, Bounds.Bottom - handleSize / 2, handleSize, handleSize), // Bottom-right
                new RectangleF(Bounds.Left - handleSize / 2, Bounds.Bottom - handleSize / 2, handleSize, handleSize) // Bottom-left
            ];
        }
    }
}
