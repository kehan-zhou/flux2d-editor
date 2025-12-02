using Flux2DEditor.Core.Geometry;
using Flux2DEditor.Core.Models;

namespace Flux2DEditor.Core.Commands
{
    /// <summary>
    /// Resizes a shape from its old bounds to new bounds.
    /// Suppots undo/redo.
    /// </summary>
    public class ResizeShapeCommand : ICommand
    {
        private readonly IShape _shape;
        private readonly BoundingBox _oldBounds;
        private readonly BoundingBox _newBounds;

        public string Description => "Resize Shape";

        public ResizeShapeCommand(IShape shape, BoundingBox oldBounds, BoundingBox newBounds)
        {
            _shape = shape;
            _oldBounds = oldBounds;
            _newBounds = newBounds;
        }

        public void Execute()
        {
            _shape.SetBounds(_newBounds);
        }
        
        public void Undo()
        {
            _shape.SetBounds(_oldBounds);
        }
    }
}