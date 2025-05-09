using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux2DEditor.Core.Shapes
{
    public class CircleShape : BaseShape
    {
        public RectangleF Bounds { get; private set; }

        public CircleShape(RectangleF bounds)
        {
            Bounds = bounds;
        }

        public override void Draw(Graphics g)
        {
            using var pen = new Pen(IsSelected ? Color.Blue : Color.Black, 2);
            g.DrawEllipse(pen, Bounds);

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
            var center = new PointF(Bounds.X + Bounds.Width / 2, Bounds.Y + Bounds.Height / 2);
            var radius = Bounds.Width / 2;
            var distance = Math.Sqrt(Math.Pow(point.X - center.X, 2) + Math.Pow(point.Y - center.Y, 2));
            return distance <= radius;
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
            var centerX = Bounds.X + Bounds.Width / 2;
            var centerY = Bounds.Y + Bounds.Height / 2;

            float radius = Bounds.Width / 2; // Current radius
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
            Bounds = new RectangleF(
                centerX - newRadius,
                centerY - newRadius,
                newRadius * 2,
                newRadius * 2
            );
        }

        private RectangleF[] GetHandles()
        {
            const float handleSize = 8f;
            var centerX = Bounds.X + Bounds.Width / 2;
            var centerY = Bounds.Y + Bounds.Height / 2;

            return new[]
            {
                new RectangleF(centerX - handleSize / 2, Bounds.Top - handleSize / 2, handleSize, handleSize), // Top
                new RectangleF(Bounds.Right - handleSize / 2, centerY - handleSize / 2, handleSize, handleSize), // Right
                new RectangleF(centerX - handleSize / 2, Bounds.Bottom - handleSize / 2, handleSize, handleSize), // Bottom
                new RectangleF(Bounds.Left - handleSize / 2, centerY - handleSize / 2, handleSize, handleSize) // Left
            };
        }
    }
}
