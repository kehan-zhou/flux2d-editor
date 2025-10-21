using Flux2DEditor.Core.Models;

namespace Flux2DEditor.Core.Commands
{
    /// <summary>
    /// Deletes the currently selected shape from the editor.
    /// </summary>
    public class DeleteShapeCommand : ICommand
    {
        #region Fields

        private readonly EditorState _editor;
        private readonly IShape _shape;

        #endregion

        public string Description => "Delete Shape";

        public DeleteShapeCommand(EditorState editor, IShape shape)
        {
            _editor = editor;
            _shape = shape;
        }

        public void Execute()
        {
            _editor.Shapes.Remove(_shape);
            if (_editor.SelectedShape == _shape)
            {
                _editor.SelectShape(null);
            }
        }

        public void Undo()
        {
            _editor.Shapes.Add(_shape);
            _editor.SelectShape(_shape);
        }
    }
}