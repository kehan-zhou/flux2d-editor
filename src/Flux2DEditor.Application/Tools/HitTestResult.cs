using Flux2DEditor.Domain.Handles;
using Flux2DEditor.Domain.Shapes;

namespace Flux2DEditor.Application.Tools
{
    public sealed class HitTestResult
    {
        public ShapeId? HitShapeId { get; }
        public HandleHitResult? HandleHit { get; }

        private HitTestResult(ShapeId? hitShapeId, HandleHitResult? handleHit)
        {
            HitShapeId = hitShapeId;
            HandleHit = handleHit;
        }

        public bool IsHandle => HandleHit != null;
        public bool IsShape => HitShapeId != null;
        public bool IsMiss => HitShapeId == null && HandleHit == null;

        public ShapeId? HandleShapeId => HandleHit?.ShapeId;
        public HandleType? HandleType => HandleHit?.HandleType;

        public static HitTestResult Shapa(ShapeId shapeId) => new(shapeId, null);

        public static HitTestResult Handle(HandleHitResult handle) => new(null, handle);

        public static HitTestResult Miss() => new(null, null);
    }
}
