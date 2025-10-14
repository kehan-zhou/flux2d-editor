using Flux2DEditor.Core.Interfaces;

namespace Flux2DEditor.Core
{
    /// <summary>
    /// Holds document-level editor state: shapes and selection.
    /// This is now a normal class (not a singleton) to allow multiple documents and testing.
    /// </summary>
    public class EditorState
    {
        #region Properties

        /// <summary>
        /// All shapes in the document, in drawing order (back to front).
        /// </summary>
        public List<IShape> Shapes { get; } = new List<IShape>();

        /// <summary>
        /// Currently selected shape (null if none).
        /// </summary>
        public IShape? SelectedShape { get; private set; }

        #endregion

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
                s.IsSelected = (s == shape);
            }
            SelectedShape = shape;
        }

        /// <summary>
        /// Adds a shape and selects it.
        /// </summary>
        /// <param name="shape">Shape to add.</param>
        public void AddShape(IShape shape)
        {
            Shapes.Add(shape);
            SelectShape(shape);
        }

        /// <summary>
        /// Removes the selected shape.
        /// </summary>
        public void DeleteSelectedShape()
        {
            if (SelectedShape != null)
            {
                Shapes.Remove(SelectedShape);
                SelectedShape = null;
            }
        }

        /// <summary>
        /// Finds the top-most shape under the given point (world coordinates).
        /// </summary>
        /// <param name="point">Point in world coordinates.</param>
        /// <returns>Top-most shape or null.</returns>
        public IShape? FindShapeAt(PointF point)
        {
            return Shapes.AsEnumerable().Reverse().FirstOrDefault(s => s.HitTest(point));
        }

        #endregion
    }
}
