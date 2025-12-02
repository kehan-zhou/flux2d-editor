using Flux2DEditor.Core.Geometry;
using Flux2DEditor.Core.Rendering;
using System.Numerics;

namespace Flux2DEditor.Core.Models
{
    /// <summary>
    /// Represents a geometric shape in the editor.
    /// The shape is a UI-agnostic model: it exposes geometry and editing operations
    /// but contains no rendering or UI-specific code.
    /// </summary>
    public interface IShape
    {
        #region Properties

        /// <summary>
        /// Gets or sets whether the shape is currently selected.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets the bounding box of the shape in world coordinates.
        /// </summary>
        BoundingBox Bounds { get; }

        #endregion

        #region  Editing API

        /// <summary>
        /// Tests whether the given point (in world coordinates) hits the shape.
        /// </summary>
        /// <param name="point">Point in world coordinates.</param>
        /// <returns>True if hit.</returns>
        bool HitTest(Vector2 point);

        /// <summary>
        /// Moves the shape by the given offset (in world coordinates).
        /// </summary>
        /// <param name="offset">Offset in world coordinates.</param>
        void Move(Vector2 offset);

        /// <summary>
        /// Tests whether the given point hits any control handle of the shape.
        /// </summary>
        /// <param name="point">Point in world coordinates.</param>
        /// <param name="handleIndex">Out handle index when hit.</param>
        /// <returns>True if a handle was hit.</returns>
        bool HitTestHandle(Vector2 point, out int handleIndex);

        /// <summary>
        /// Resizes the shape by dragging a handle identified by index to a new point.
        /// </summary>
        /// <param name="handleIndex">Handle index.</param>
        /// <param name="newPoint">New point in world coordinates.</param>
        void ResizeFromHandle(int handleIndex, Vector2 newPoint);

        /// <summary>
        /// Updates the shape geometry to fit the specified bounding rectangle.
        /// </summary>
        /// <param name="newBounds">New bounds in world coordinates.</param>
        void SetBounds(BoundingBox newBounds);

        /// <summary>
        /// Creates a deep copy of this shape.
        /// Used for clipboard operations.
        /// </summary>
        /// <returns>A new shape instance with identical geometry and state.</returns>
        IShape Clone();

        /// <summary>
        /// Draws the shape using the provided rendering context and styles.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="style"></param>
        void Draw(IRenderContext context, RenderStyles style);

        #endregion
    }
}
