using Flux2DEditor.Core.Editor;
using Flux2DEditor.Core.Models;

namespace Flux2DEditor.Core.Commands
{
    /// <summary>
    /// Adds a new shape to the editor.
    /// </summary>
    public class AddShapeCommand : ICommand
    {
        #region Fields
        
        private readonly EditorState _editor;
        private readonly IShape _shape;

        #endregion

        public string Description => "Add Shape";

        public AddShapeCommand(EditorState editor, IShape shape)
        {
            _editor = editor;
            _shape = shape;
        }

        public void Execute()
        {
            _editor.Shapes.Add(_shape);
            _editor.SelectShape(_shape);
        }

        public void Undo()
        {
            _editor.Shapes.Remove(_shape);
            _editor.SelectShape(null);
        }
    }
}