using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux2DEditor.Core.Rendering
{
    /// <summary>
    /// Abstract renderer that prepares a render context for drawing on a target device.
    /// A device could be a WinForms control, a bitmap, or an offscreen buffer.
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Begins a drawing session and returns a render context for drawing operations.
        /// </summary>
        /// <returns></returns>
        IRenderContext BeginDraw();

        /// <summary>
        /// Ends the drawing session, finalizing all drawing operations.
        /// </summary>
        void EndDraw();
    }
}
