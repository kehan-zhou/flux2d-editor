using Flux2DEditor.Domain.Geometry;
using Flux2DEditor.Domain.Handles;

namespace Flux2DEditor.Domain.Shapes
{
    public sealed class Rectangle : Shape, IHandleProvider, IResizableShape
    {
        public Vector2 Position { get; }
        public Vector2 Size { get; }

        public Rectangle(ShapeId id, Vector2 position, Vector2 size) : base(id)
        {
            Position = position;
            Size = size;
        }

        public override BoundingBox GetBoundingBox() 
            => BoundingBox.FromPositionAndSize(Position, Size);

        public override bool HitTest(Vector2 point, double tolerance)
        {
            var box = GetBoundingBox();
            return box.Contains(point);
        }

        public override Shape Translate(Vector2 delta)
            => new Rectangle(Id, Position + delta, Size);

        public override Shape Clone() 
            => new Rectangle(ShapeId.New(), Position, Size);

        public IEnumerable<Handle> GetHandles()
        {
            var topLeft = Position;
            var topRight = new Vector2(Position.X + Size.X, Position.Y);
            var bottomLeft = new Vector2(Position.X, Position.Y + Size.Y);
            var bottomRight = Position + Size;

            yield return new Handle(HandleType.RectTopLeft, topLeft);
            yield return new Handle(HandleType.RectTopRight, topRight);
            yield return new Handle(HandleType.RectBottomLeft, bottomLeft);
            yield return new Handle(HandleType.RectBottomRight, bottomRight);
        }

        public Shape Resize(HandleType handle, Vector2 worldPosition)
        {
            var topLeft = Position;
            var bottomRight = Position + Size;

            Vector2 newTopLeft = topLeft;
            Vector2 newBottomRight = bottomRight;

            switch (handle)
            {
                case HandleType.RectTopLeft:
                    newTopLeft = worldPosition;
                    break;

                case HandleType.RectTopRight:
                    newTopLeft = new Vector2(topLeft.X, worldPosition.Y);
                    newBottomRight = new Vector2(worldPosition.X, bottomRight.Y);
                    break;

                case HandleType.RectBottomLeft:
                    newTopLeft = new Vector2(worldPosition.X, topLeft.Y);
                    newBottomRight = new Vector2(bottomRight.X, worldPosition.Y);
                    break;

                case HandleType.RectBottomRight:
                    newBottomRight = worldPosition;
                    break;

                default:
                    return this;
            }

            var min = Vector2.Min(newTopLeft, newBottomRight);
            var max = Vector2.Max(newTopLeft, newBottomRight);

            return new Rectangle(Id, min, max - min);
        }
    }
}
