using Flux2DEditor.Domain.Shapes;
using Flux2DEditor.Domain.Geometry;

namespace Flux2DEditor.Application.Editor
{
    public sealed class MoveContext
    {
        public ShapeId ShapeId { get; }
        public Vector2 StartPosition { get; }
        public Vector2 GrabOffset { get; }
        public Vector2 CurrentDelta { get; private set; }

        public MoveContext(ShapeId shapeId, Vector2 startPosition, Vector2 grabOffset)
        {
            ShapeId = shapeId;
            StartPosition = startPosition;
            GrabOffset = grabOffset;
            CurrentDelta = Vector2.Zero;
        }

        public void Update(Vector2 currentWorldPosition)
        {
            var newTopLeft = currentWorldPosition - GrabOffset;
            CurrentDelta = newTopLeft - StartPosition;
        }
    }
}
