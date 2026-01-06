using Flux2DEditor.Domain.Geometry;
using Flux2DEditor.Domain.Scene;
using Flux2DEditor.Domain.Shapes;

namespace Flux2DEditor.Application.Commands
{
    public sealed class MoveShapeCommand : ICommand
    {
        private readonly Scene _scene;
        private readonly ShapeId _shapeId;
        private readonly Vector2 _delta;

        private Shape? _before;
        private Shape? _after;

        public MoveShapeCommand(Scene scene, ShapeId shapeId, Vector2 delta)
        {
            _scene = scene;
            _shapeId = shapeId;
            _delta = delta;
        }

        public void Execute()
        {
            if (_after == null)
            {
                _before = _scene.Get(_shapeId);
                _after = Move(_before, _delta);
            }

            _scene.Replace(_after);
        }

        public void Undo()
        {
            if (_before == null)
            {
                return;
            }

            _scene.Replace(_before);
        }

        private static Shape Move(Shape shape, Vector2 delta)
        {
            return shape.Translate(delta);
        }
    }
}
