using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux2DEditor.Core
{
    public class RectangleObject : SelectableObject
    {
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

                PointF[] corners = new PointF[]
                {
                    new PointF(Bounds.Left, Bounds.Top),
                    new PointF(Bounds.Right, Bounds.Top),
                    new PointF(Bounds.Right, Bounds.Bottom),
                    new PointF(Bounds.Left, Bounds.Bottom)
                };

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
    }
}
