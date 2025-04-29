using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux2DEditor.Core
{
    public class EditorRectangle
    {
        public RectangleF Bounds { get; set; }
        public Color Color { get; set; } = Color.Blue;

        public EditorRectangle(RectangleF bounds)
        {
            Bounds = bounds;
        }
    }
}
