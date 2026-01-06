using Flux2DEditor.Application.Tools;
using Flux2DEditor.Domain.Geometry;
using Flux2DEditor.Domain.Scene;
using Flux2DEditor.Domain.Shapes;

namespace Flux2DEditor.Application.HitTesting
{
    public sealed class HitTestService
    {
        public HitTestResult HitTest(Scene scene, Vector2 point)
        {
            foreach (Shape shape in scene.Shapes)
            {
                BoundingBox bounds = shape.GetBoundingBox();

                if (bounds.Contains(point))
                {
                    return HitTestResult.Hit(shape.Id);
                }
            }

            return HitTestResult.Miss();
        }
    }
}
