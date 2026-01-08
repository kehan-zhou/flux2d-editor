using Flux2DEditor.Domain.Geometry;
using Flux2DEditor.Domain.Scene;
using Flux2DEditor.Domain.Shapes;

namespace Flux2DEditor.Application.Commands
{
    public sealed class MoveShapesCommand : ICommand
    {
        private readonly Scene _scene;
        private readonly IReadOnlyList<ShapeId> shapeIds;
        private readonly Vector2 _delta;

        private Dictionary<ShapeId, Shape>? _before;
        private Dictionary<ShapeId, Shape>? _after;

        public MoveShapesCommand(Scene scene, IReadOnlyList<ShapeId> shapeIds, Vector2 delta)
        {
            _scene = scene;
            this.shapeIds = shapeIds;
            _delta = delta;
        }

        public void Execute()
        {
            if (_after == null)
            {
                _before = new Dictionary<ShapeId, Shape>();
                _after = new Dictionary<ShapeId, Shape>();

                foreach (var id in shapeIds)
                {
                    var shape = _scene.Get(id);
                    _before[id] = shape;
                    _after[id] = Move(shape, _delta);
                }
            }

            foreach (var shape in _after.Values)
            {
                _scene.Replace(shape);
            }
        }

        public void Undo()
        {
            if (_before == null) return;

            foreach (var shape in _before.Values)
            {
                _scene.Replace(shape);
            }
        }

        private static Shape Move(Shape shape, Vector2 delta)
        {
            if (shape is Rectangle rect)
                return rect.WithPosition(rect.Position + delta);

            throw new NotSupportedException();
        }
    }
}
