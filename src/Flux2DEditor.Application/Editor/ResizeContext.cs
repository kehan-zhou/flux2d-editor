using Flux2DEditor.Domain.Geometry;
using Flux2DEditor.Domain.Handles;
using Flux2DEditor.Domain.Shapes;

namespace Flux2DEditor.Application.Editor
{
    public sealed class ResizeContext
    {
        public ShapeId ShapeId { get; }
        public HandleType HandleType { get; }

        public Shape OriginalShape { get; }
        public Shape? PreviewShape { get; private set; }

        public ResizeContext(ShapeId shapeId, HandleType handleType, Shape original)
        {
            ShapeId = shapeId;
            HandleType = handleType;
            OriginalShape = original;
        }

        public void Update(Vector2 worldPosition)
        {
            PreviewShape = OriginalShape switch
            {
                LineSegment line => UpdateLine(line, worldPosition),
                _ => OriginalShape
            };
        }

        private Shape UpdateLine(LineSegment line, Vector2 pos)
        {
            return HandleType switch
            {
                HandleType.LineStart => new LineSegment(line.Id, pos, line.End),
                HandleType.LineEnd => new LineSegment(line.Id, line.Start, pos),
                _ => line
            };
        }
    }
}
