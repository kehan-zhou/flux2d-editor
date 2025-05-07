using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Flux2DEditor.Core
{
    public class RectangleObject : SelectableObject
    {
        private int _activeHandleIndex = -1;

        public RectangleF Bounds { get; private set; }

        public RectangleObject(RectangleF bounds)
        {
            Bounds = bounds;
        }

        public override void Draw(Graphics g)
        {
            if (IsSelected)
            {
                using var dashedPen = new Pen(Color.Blue, 2)
                {
                    DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
                };
                g.DrawRectangle(dashedPen, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);

                float size = 6f;
                var handleBrush = Brushes.White;
                var handlePen = Pens.Black;

                PointF[] corners = GetHandlePoints();
                foreach (var corner in corners)
                {
                    var handleRect = new RectangleF(corner.X - size/2, corner.Y - size/2, size, size);
                    g.FillRectangle(handleBrush, handleRect);
                    g.DrawRectangle(handlePen, handleRect.X, handleRect.Y, handleRect.Width, handleRect.Height);
                }
            }
            else
            {
                using var pen = new Pen(Color.Black, 2);
                g.DrawRectangle(pen, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
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

        public bool HitTestHandle(PointF point, out int handleIndex)
        {
            const float size = 6f;
            PointF[] handles = GetHandlePoints();
            for (int i = 0; i < handles.Length; i++)
            {
                var rect = new RectangleF(handles[i].X - size / 2, handles[i].Y - size / 2, size, size);
                if (rect.Contains(point))
                {
                    handleIndex = i;
                    return true;
                }
            }

            handleIndex = -1;
            return false;
        }

        public void SetActiveHandle(int index) => _activeHandleIndex = index;
        public bool HasActiveHandle() => _activeHandleIndex != -1;
        public void ClearActiveHandle() => _activeHandleIndex = -1;

        public void ResizeFromActiveHandle(PointF newPoint)
        {
            var corners = GetHandlePoints();
            var opposite = GetHandlePoints()[(_activeHandleIndex + 2) % 4];

            Bounds = new RectangleF(
                Math.Min(opposite.X, newPoint.X),
                Math.Min(opposite.Y, newPoint.Y),
                Math.Abs(opposite.X - newPoint.X),
                Math.Abs(opposite.Y - newPoint.Y)
            );
        }

        private PointF[] GetHandlePoints()
        {
            return new PointF[]
            {
                new PointF(Bounds.Left, Bounds.Top),
                new PointF(Bounds.Right, Bounds.Top),
                new PointF(Bounds.Right, Bounds.Bottom),
                new PointF(Bounds.Left, Bounds.Bottom)
            };
        }
    }
}
