using Flux2DEditor.Domain.Scene;
using Flux2DEditor.Domain.Shapes;

namespace Flux2DEditor.Application.Commands
{
    public sealed class ResizeShapeCommand : ICommand
    {
        private readonly Scene _scene;
        private readonly Shape _before;
        private readonly Shape _after;

        public ResizeShapeCommand(Scene scene, Shape before, Shape after)
        {
            _scene = scene;
            _before = before;
            _after = after;
        }

        public void Execute()
        {
            _scene.Replace(_after);
        }

        public void Undo()
        {
            _scene.Replace(_before);
        }
    }
}
