using Flux2DEditor.Core.Models;
using Flux2DEditor.Core.Services;

namespace Flux2DEditor.Core.Commands
{
    /// <summary>
    /// Cuts the selected shape to clipboard and removes it from the editor.
    /// Supports undo/redo.
    /// </summary>
    public class CutShapeCommand : ICommand
    {
        #region Fields

        private readonly EditorState _editor;
        private readonly ShapeClipboard _clipboard;
        private readonly IShape _shape;

        #endregion

        public string Description => "Cut Shape";

        public CutShapeCommand(EditorState editor, ShapeClipboard clipboard, IShape shape)
        {
            _editor = editor;
            _clipboard = clipboard;
            _shape = shape;
        }

        public void Execute()
        {
            _clipboard.Copy(_shape);
            _editor.Shapes.Remove(_shape);
            _editor.SelectShape(null);
        }

        public void Undo()
        {
            _editor.Shapes.Add(_shape);
            _editor.SelectShape(_shape);
        }
    }
}