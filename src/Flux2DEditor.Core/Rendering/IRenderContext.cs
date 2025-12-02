using Flux2DEditor.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Flux2DEditor.Core.Rendering
{
    /// <summary>
    /// Rendering context for drawing shapes using an abstract graphics backend.
    /// </summary>
    public interface IRenderContext
    {
        /// <summary>
        /// Draws a line between two points with the specified stroke style.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="stroke"></param>
        void DrawLine(Vector2 a, Vector2 b, StrokeStyle stroke);

        /// <summary>
        /// Draws a rectangle defined by the bounding box with the specified stroke style.
        /// </summary>
        /// <param name="box"></param>
        /// <param name="stroke"></param>
        void DrawRectangle(BoundingBox box, StrokeStyle stroke);

        /// <summary>
        /// Fills a rectangle defined by the bounding box with the specified fill style.
        /// </summary>
        /// <param name="box"></param>
        /// <param name="fill"></param>
        void FillRectangle(BoundingBox box, FillStyle fill);

        /// <summary>
        /// Draws a circle at the specified center with the given radius and stroke style.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="stroke"></param>
        void DrawCircle(Vector2 center, float radius, StrokeStyle stroke);

        /// <summary>
        /// Fills a circle at the specified center with the given radius and fill style.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="fill"></param>
        void FillCircle(Vector2 center, float radius, FillStyle fill);

        /// <summary>
        /// Draws a polyline defined by the given points. If 'closed' is true, connects the last point to the first.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="closed"></param>
        /// <param name="stroke"></param>
        void DrawPolyline(ReadOnlySpan<Vector2> points, bool closed, StrokeStyle stroke);

        /// <summary>
        /// Fills a polygon defined by the given points with the specified fill style.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="fill"></param>
        void FillPolygon(ReadOnlySpan<Vector2> points, FillStyle fill);

        /// <summary>
        /// Draws text at the specified position with the given color and size.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="size"></param>
        void DrawText(string text, Vector2 position, Vector4 color, float size = 12f);
    }
}
