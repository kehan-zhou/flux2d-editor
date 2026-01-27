using Flux2DEditor.Domain.Handles;
using Flux2DEditor.Domain.Shapes;

namespace Flux2DEditor.Application.Tools
{
    public sealed class HandleHitResult
    {
        public ShapeId ShapeId { get; }
        public HandleType HandleType { get; }

        public HandleHitResult(ShapeId shapeId, HandleType handleType)
        {
            ShapeId = shapeId;
            HandleType = handleType;
        }
    }
}
