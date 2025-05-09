using Flux2DEditor.Core.Interfaces;
using Flux2DEditor.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux2DEditor.Core
{
    public static class ShapeFactory
    {
        public static IShape CreateShape(string type, RectangleF bounds)
        {
            return type switch
            {
                "Rectangle" => new RectangleShape(bounds),
                _ => throw new ArgumentException($"Unknown shape type: {type}")
            };
        }
    }
}
