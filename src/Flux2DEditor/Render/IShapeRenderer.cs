using Flux2DEditor.Core.Models;

namespace Flux2DEditor.Render
{
    /// <summary>
    /// Rendering abstraction to draw shapes into a Graphics context.
    /// Keeps rendering logic out of the shape model classes.
    /// </summary>
    public interface IShapeRenderer
    {
        /// <summary>
        /// Rendering a shape into the provided Graphics context.
        /// </summary>
        /// <param name="shape">Shape to render.</param>
        /// <param name="g">Graphics to draw into (already transformed by viewport).</param>
        void Render(IShape shape, Graphics g);
    }
}