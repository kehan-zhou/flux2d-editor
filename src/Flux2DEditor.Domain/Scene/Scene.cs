using Flux2DEditor.Domain.Shapes;

namespace Flux2DEditor.Domain.Scene
{
    public sealed class Scene
    {
        private readonly Dictionary<ShapeId, Shape> _shapes = [];

        public IReadOnlyCollection<Shape> Shapes => _shapes.Values;

        public void Add(Shape shape)
        {
            if (_shapes.ContainsKey(shape.Id))
            {
                throw new InvalidOperationException("Shape already exists in scene.");
            }

            _shapes.Add(shape.Id, shape);
        }

        public void Remove(ShapeId id)
        {
            if (!_shapes.Remove(id))
            {
                throw new InvalidOperationException("Shape does not exist in scene.");
            }
        }

        public Shape Get(ShapeId id)
        {
            return _shapes[id];
        }

        public bool Contains(ShapeId id)
        {
            return _shapes.ContainsKey(id);
        }

        public void Replace(Shape shape)
        {
            if (!_shapes.ContainsKey(shape.Id))
            {
                throw new InvalidOperationException("Shape does not exist in scene.");
            }

            _shapes[shape.Id] = shape;
        }
    }
}
