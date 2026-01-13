using Flux2DEditor.Domain.Shapes;
using Flux2DEditor.Domain.Geometry;

namespace Flux2DEditor.Application.Editor
{
    public sealed class MoveContext
    {
        public IReadOnlyList<ShapeId> ShapeIds { get; }
        public IReadOnlyList<Shape>? PreviewCopies { get; private set; }
        public bool IsCopy { get; }
        public Vector2 StartPosition { get; }
        public Vector2 GrabOffset { get; }
        public Vector2 CurrentDelta { get; private set; }

        public MoveContext(IReadOnlyList<ShapeId> shapeIds, Vector2 startPosition, Vector2 grabOffset, bool isCopy)
        {
            ShapeIds = shapeIds;
            StartPosition = startPosition;
            GrabOffset = grabOffset;
            CurrentDelta = Vector2.Zero;
            IsCopy = isCopy;
        }

        public void SetPreviewCopies(IReadOnlyList<Shape> copies)
        {
            PreviewCopies = copies;
        }

        public void Update(Vector2 currentWorldPosition)
        {
            var newTopLeft = currentWorldPosition - GrabOffset;
            CurrentDelta = newTopLeft - StartPosition;
        }
    }
}
