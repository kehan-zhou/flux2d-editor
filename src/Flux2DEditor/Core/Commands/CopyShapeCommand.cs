using Flux2DEditor.Core.Models;
using Flux2DEditor.Core.Services;

namespace Flux2DEditor.Core.Commands
{
    /// <summary>
    /// Copies the selected shape to clipboard.
    /// </summary>
    public class CopyShapeCommand : ICommand
    {
        #region Fields

        private readonly EditorState _editor;
        private readonly ShapeClipboard _clipboard;

        #endregion

        public string Description => "Copy Shape";

        public CopyShapeCommand(EditorState editor, ShapeClipboard clipboard)
        {
            _editor = editor;
            _clipboard = clipboard;
        }

        public void Execute()
        {
            if (_editor.SelectedShape != null)
            {
                _clipboard.Copy(_editor.SelectedShape);
            }
        }

        public void Undo()
        {
            // Copy operation does not modify state, so nothing to undo.
        }
    }
}

