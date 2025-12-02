using Flux2DEditor.Core.Commands;
using Flux2DEditor.Core.Geometry;
using Flux2DEditor.Core.Models;
using Flux2DEditor.Core.Services;
using System.Numerics;

namespace Flux2DEditor.Core.Editor
{
    /// <summary>
    /// Holds document-level editor state: shapes and selection.
    /// This is now a normal class (not a singleton) to allow multiple documents and testing.
    /// </summary>
    public class EditorState
    {
        /// <summary>
        /// All shapes in the document, in drawing order (back to front).
        /// </summary>
        public List<IShape> Shapes { get; } = [];

        /// <summary>
        /// Currently selected shape (null if none).
        /// </summary>
        public IShape? SelectedShape { get; private set; }

        public CommandManager Commands { get; } = new();

        public ShapeClipboard Clipboard { get; } = new();

        #region Public API

        /// <summary>
        /// Selects the provided shape (or deselects when null).
        /// Ensures only one shape is selected at a time.
        /// </summary>
        /// <param name="shape">Shape to select or null.</param>
        public void SelectShape(IShape? shape)
        {
            foreach (var s in Shapes)
            {
                s.IsSelected = s == shape;
            }
            SelectedShape = shape;
        }

        /// <summary>
        /// Adds a shape and selects it.
        /// </summary>
        /// <param name="shape">Shape to add.</param>
        public void AddShape(IShape shape)
        {
            Commands.ExecuteCommand(new AddShapeCommand(this, shape));
        }

        /// <summary>
        /// Removes the selected shape.
        /// </summary>
        public void DeleteSelectedShape()
        {
            if (SelectedShape != null)
            {
                Commands.ExecuteCommand(new DeleteShapeCommand(this, SelectedShape));
            }
        }

        public void MoveSelectedShape(Vector2 offset)
        {
            if (SelectedShape != null)
            {
                Commands.ExecuteCommand(new MoveShapeCommand(SelectedShape, offset));
            }
        }

        /// <summary>
        /// Finds the top-most shape under the given point (world coordinates).
        /// </summary>
        /// <param name="point">Point in world coordinates.</param>
        /// <returns>Top-most shape or null.</returns>
        public IShape? FindShapeAt(Vector2 point)
        {
            return Shapes.AsEnumerable().Reverse().FirstOrDefault(s => s.HitTest(point));
        }

        public void ResizeSelectedShape(BoundingBox oldBounds, BoundingBox newBounds)
        {
            if (SelectedShape != null)
            {
                Commands.ExecuteCommand(new ResizeShapeCommand(SelectedShape, oldBounds, newBounds));
            }
        }

        public void CopySelectedShape()
        {
            Commands.ExecuteCommand(new CopyShapeCommand(this, Clipboard));
        }

        public void CutSelectedShape()
        {
            if (SelectedShape != null)
            {
                Commands.ExecuteCommand(new CutShapeCommand(this, Clipboard, SelectedShape));
            }
        }

        public void PasteShape()
        {
            Commands.ExecuteCommand(new PasteShapeCommand(this, Clipboard));
        }

        #endregion
    }
}
