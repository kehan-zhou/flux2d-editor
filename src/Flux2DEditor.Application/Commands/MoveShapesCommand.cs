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
                _before = [];
                _after = [];

                foreach (var id in shapeIds)
                {
                    var original = _scene.Get(id);
                    _before[id] = original;
                    _after[id] = original.Translate(_delta);
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
    }
}
