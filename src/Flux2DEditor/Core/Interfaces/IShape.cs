using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux2DEditor.Core.Interfaces
{
    public interface IShape
    {
        bool IsSelected { get; set; }
        void Draw(Graphics g);
        bool HitTest(PointF point);
        void Move(PointF offset);
        bool HitTestHandle(PointF point, out int handleIndex);
        void ResizeFromHandle(int handleIndex, PointF newPoint);
    }
}
