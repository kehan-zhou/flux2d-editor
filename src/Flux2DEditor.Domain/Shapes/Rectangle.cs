using Flux2DEditor.Domain.Geometry;

namespace Flux2DEditor.Domain.Shapes
{
    public sealed class Rectangle : Shape
    {
        public Vector2 Position { get; }
        public Vector2 Size { get; }

        public Rectangle(ShapeId id, Vector2 position, Vector2 size) : base(id)
        {
            Position = position;
            Size = size;
        }

        public Rectangle WithPosition(Vector2 newPosition) => new Rectangle(Id, newPosition, Size);

        public override BoundingBox GetBoundingBox() => BoundingBox.FromPositionAndSize(Position, Size);

        public override Shape Translate(Vector2 delta)
        {
            return WithPosition(Position + delta);
        }

        public override Shape Clone() => new Rectangle(ShapeId.New(), Position, Size);
    }
}
