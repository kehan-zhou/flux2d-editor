namespace Flux2DEditor.Core.Models
{
    /// <summary>
    /// Represents a geometric shape in the editor.
    /// The Shape contains geometry and editing behavior but does NOT perform rendering.
    /// </summary>
    public interface IShape
    {
        #region Properties

        /// <summary>
        /// Gets or sets whether the shape is currently selected.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets the bounding rectangle of the shape in world coordinates.
        /// </summary>
        RectangleF Bounds { get; }

        #endregion

        #region  Editing API

        /// <summary>
        /// Tests whether the given point (in world coordinates) hits the shape.
        /// </summary>
        /// <param name="point">Point in world coordinates.</param>
        /// <returns>True if hit.</returns>
        bool HitTest(PointF point);

        /// <summary>
        /// Moves the shape by the given offset (in world coordinates).
        /// </summary>
        /// <param name="offset">Offset in world coordinates.</param>
        void Move(PointF offset);

        /// <summary>
        /// Tests whether the given point hits any control handle of the shape.
        /// </summary>
        /// <param name="point">Point in world coordinates.</param>
        /// <param name="handleIndex">Out handle index when hit.</param>
        /// <returns>True if a handle was hit.</returns>
        bool HitTestHandle(PointF point, out int handleIndex);

        /// <summary>
        /// Resizes the shape by dragging a handle identified by index to a new point.
        /// </summary>
        /// <param name="handleIndex">Handle index.</param>
        /// <param name="newPoint">New point in world coordinates.</param>
        void ResizeFromHandle(int handleIndex, PointF newPoint);

        /// <summary>
        /// Updates the shape geometry to fit the specified bounding rectangle.
        /// </summary>
        /// <param name="newBounds">New bounds in world coordinates.</param>
        void SetBounds(RectangleF newBounds);

        /// <summary>
        /// Creates a deep copy of this shape.
        /// Used for clipboard operations.
        /// </summary>
        /// <returns>A new shape instance with identical geometry and state.</returns>
        IShape Clone();

        #endregion
    }
}
