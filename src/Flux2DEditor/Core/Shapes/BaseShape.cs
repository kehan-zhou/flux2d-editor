using Flux2DEditor.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux2DEditor.Core.Shapes
{
    public abstract class BaseShape : IShape
    {
        public bool IsSelected { get; set; } = false;

        public abstract void Draw(Graphics g);
        public abstract bool HitTest(PointF point);
        public abstract void Move(PointF offset);
        public abstract bool HitTestHandle(PointF point, out int handleIndex);
        public abstract void ResizeFromHandle(int handleIndex, PointF newPoint);
    }
}
