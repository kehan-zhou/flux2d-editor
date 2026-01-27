using Flux2DEditor.Application.Tools;
using Flux2DEditor.Domain.Geometry;
using Flux2DEditor.Domain.Scene;
using Flux2DEditor.Domain.Shapes;

namespace Flux2DEditor.Application.HitTesting
{
    public sealed class HitTestService
    {
        private const double HandleHitRadius = 6.0;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
        public HitTestResult HitTest(Scene scene, Vector2 point)
        {
            foreach (Shape shape in scene.Shapes.Reverse())
            {
                if (shape is IHandleProvider handleProvider)
                {
                    foreach (var handle in handleProvider.GetHandles())
                    {
                        var delta = point - handle.Position;
                        if (delta.LengthSquared() <= HandleHitRadius * HandleHitRadius)
                        {
                            return HitTestResult.Handle(new HandleHitResult(shape.Id, handle.Type));
                        }
                    }
                }

                if (shape.HitTest(point))
                {
                    return HitTestResult.Shapa(shape.Id);
                }
            }

            return HitTestResult.Miss();
        }
    }
}
