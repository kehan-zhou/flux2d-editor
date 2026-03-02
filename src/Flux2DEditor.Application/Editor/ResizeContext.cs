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
            if (OriginalShape is IResizableShape resizable)
            {
                PreviewShape = resizable.Resize(HandleType, worldPosition);
            }
            else
            {
                PreviewShape = OriginalShape;
            }
        }
    }
}
