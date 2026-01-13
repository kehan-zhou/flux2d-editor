using Flux2DEditor.Domain.Scene;
using Flux2DEditor.Domain.Shapes;

namespace Flux2DEditor.Application.Commands
{
    public sealed class CopyShapesCommand : ICommand
    {
        private readonly Scene _scene;
        private readonly IReadOnlyList<Shape> _copies;

        public IReadOnlyList<Shape> Copies => _copies;

        public CopyShapesCommand(Scene scene, IReadOnlyList<Shape> copies)
        {
            _scene = scene;
            _copies = copies;
        }

        public void Execute()
        {
            foreach (var copy in _copies)
            {
                _scene.Add(copy);
            }
        }

        public void Undo()
        {
            foreach (var shape in _copies)
            {
                _scene.Remove(shape.Id);
            }
        }
    }
}
