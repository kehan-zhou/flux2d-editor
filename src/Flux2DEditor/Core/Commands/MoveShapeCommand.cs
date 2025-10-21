using System.Drawing.Drawing2D;
using Flux2DEditor.Core.Models;

namespace Flux2DEditor.Core.Commands
{
    /// <summary>
    /// Moves a shape by a given offset. Supports undo/redo.
    /// </summary>
    public class MoveShapeCommand : ICommand
    {
        #region Fields

        private readonly IShape _shape;
        private readonly PointF _offset;

        #endregion

        public string Description => "Move Shape";

        public MoveShapeCommand(IShape shape, PointF offset)
        {
            _shape = shape;
            _offset = offset;
        }

        public void Execute()
        {
            _shape.Move(_offset);
        }

        public void Undo()
        {
            _shape.Move(new PointF(-_offset.X, -_offset.Y));
        }
    }
}