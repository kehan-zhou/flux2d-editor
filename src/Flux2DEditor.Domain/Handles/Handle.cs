using Flux2DEditor.Domain.Geometry;

namespace Flux2DEditor.Domain.Handles
{
    public readonly struct Handle
    {
        public HandleType Type { get; }
        public Vector2 Position { get; }

        public Handle(HandleType type, Vector2 position)
        {
            Type = type;
            Position = position;
        }
    }
}
