using Flux2DEditor.Core.Editor;
using Flux2DEditor.Core.Models;
using Flux2DEditor.Core.Services;
using System.Numerics;

namespace Flux2DEditor.Core.Commands
{
    ///<summary>
    /// Pastes a shape from clipboard into the editor.
    /// </summary>
    public class PasteShapeCommand : ICommand
    {
        #region Fields

        private readonly EditorState _editor;
        private readonly ShapeClipboard _clipboard;
        private IShape _pastedShape;

        #endregion

        public string Description => "Paste Shape";

        public PasteShapeCommand(EditorState editor, ShapeClipboard clipboard)
        {
            _editor = editor;
            _clipboard = clipboard;
        }

        public void Execute()
        {
            if (_clipboard.HasShape)
            {
                _pastedShape = _clipboard.Paste();
                if (_pastedShape != null)
                {
                    var bounds = _pastedShape.Bounds;
                    _pastedShape.Move(new Vector2(10, 10));
                    _editor.Shapes.Add(_pastedShape);
                    _editor.SelectShape(_pastedShape);
                }
            }
        }

        public void Undo()
        {
            if (_pastedShape != null)
            {
                _editor.Shapes.Remove(_pastedShape);
                _editor.SelectShape(null);
            }
        }
    }
}