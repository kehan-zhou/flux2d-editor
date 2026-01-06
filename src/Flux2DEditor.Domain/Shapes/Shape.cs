using Flux2DEditor.Domain.Geometry;

namespace Flux2DEditor.Domain.Shapes
{
    public abstract class Shape : ISpatialEntity
    {
        public ShapeId Id { get; }

        protected Shape(ShapeId id)
        {
            Id = id;
        }

        public abstract BoundingBox GetBoundingBox();

        public abstract Shape Translate(Vector2 delta);

        public abstract Shape Clone();
    }
}
