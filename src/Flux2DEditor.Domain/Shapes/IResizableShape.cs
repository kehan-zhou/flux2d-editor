using Flux2DEditor.Domain.Geometry;
using Flux2DEditor.Domain.Handles;

namespace Flux2DEditor.Domain.Shapes
{
    public interface IResizableShape
    {
        Shape Resize(HandleType handle, Vector2 worldPosition);
    }
}
