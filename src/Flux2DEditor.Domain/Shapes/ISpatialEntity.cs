using Flux2DEditor.Domain.Geometry;

namespace Flux2DEditor.Domain.Shapes
{
    public interface ISpatialEntity
    {
        BoundingBox GetBoundingBox();
    }
}
