using Flux2DEditor.Domain.Shapes;

namespace Flux2DEditor.Application.Tools
{
    public sealed class HitTestResult
    {
        public ShapeId? HitShapeId { get; }

        private HitTestResult(ShapeId? hitShapeId)
        {
            HitShapeId = hitShapeId;
        }

        public static HitTestResult Hit(ShapeId shapeId) => new(shapeId);

        public static HitTestResult Miss() => new(null);
    }
}
