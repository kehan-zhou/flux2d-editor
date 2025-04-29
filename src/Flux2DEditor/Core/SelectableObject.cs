using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux2DEditor.Core
{
    public abstract class SelectableObject
    {
        public bool IsSelected { get; set; } = false;

        public abstract void Draw(Graphics g);
        public abstract bool HitTest(PointF point);
        public abstract void Move(PointF offset);
    }
}
