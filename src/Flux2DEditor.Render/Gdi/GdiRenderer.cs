using System;
using System.Drawing;
using Flux2DEditor.Core.Rendering;

namespace Flux2DEditor.Render.Gdi
{
    public sealed class GdiRenderer : IRenderer, IDisposable
    {
        private readonly Graphics _graphics;
        private bool _disposed;

        public GdiRenderer(Graphics graphics)
        {
            _graphics = graphics ?? throw new ArgumentNullException(nameof(graphics));
        }

        public IRenderContext BeginDraw()
        {
            return new GdiRenderContext(_graphics);
        }

        public void EndDraw()
        {

        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
        }
    }
}
