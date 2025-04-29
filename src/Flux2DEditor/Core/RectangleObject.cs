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
            using var pen = new Pen(IsSelected ? Color.Blue : Color.Black, 2);
            g.DrawRectangle(pen, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
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
