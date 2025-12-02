using Flux2DEditor.Core.Models;

namespace Flux2DEditor.Core.Services
{
    /// <summary>
    /// Clipboard service to copy/paste shapes.
    /// </summary>
    public class ShapeClipboard
    {
        public IShape _copiedShape;

        public bool HasShape => _copiedShape != null;

        public void Copy(IShape shape)
        {
            _copiedShape = shape.Clone();
        }

        public IShape Paste()
        {
            return _copiedShape?.Clone();
        }

        public void Clear()
        {
            _copiedShape = null;
        }
    }
}