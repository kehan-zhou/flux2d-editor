using Flux2DEditor.Core;

namespace Flux2DEditor.Forms
{
    public partial class MainForm : Form
    {
        private PointF _startPoint = PointF.Empty;
        private PointF _dragStartPoint = PointF.Empty;
        private RectangleF _drawingRect = Rectangle.Empty;

        private bool _isDrawing = false;
        private bool _isDraggingObject = false;
        private bool _isResizing = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                DeleteSelectedObject();
        }

        private void viewportMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var worldPoint = viewportMain.ScreenToWorld(e.Location);
            HandleObjectSelection(worldPoint);
            viewportMain.Invalidate();
        }

        private void viewportMain_MouseMove(object sender, MouseEventArgs e)
        {
            var worldPoint = viewportMain.ScreenToWorld(e.Location);

            if (_isDrawing)
                UpdateDrawingRectangle(worldPoint);

            if (_isDraggingObject)
                DragSelectedObject(worldPoint);

            if (_isResizing && EditorState.Instance.SelectedObject is RectangleObject rectObj)
            {
                rectObj.ResizeFromActiveHandle(worldPoint);
                viewportMain.Invalidate();
            }
        }

        private void viewportMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
                FinalizeDrawing();

            if (_isDraggingObject)
                _isDraggingObject = false;

            if (_isResizing && EditorState.Instance.SelectedObject is RectangleObject rectObj)
            {
                rectObj.ClearActiveHandle();
                _isResizing = false;
            }

            viewportMain.Invalidate();
        }

        private void viewportMain_Render(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            foreach (var obj in EditorState.Instance.Objects)
                obj.Draw(g);

            if (_drawingRect != Rectangle.Empty)
            {
                using var redPen = new Pen(Color.Red, 2);
                g.DrawRectangle(redPen, _drawingRect);
            }
        }

        // === Helper Methods ===

        private void DeleteSelectedObject()
        {
            var selected = EditorState.Instance.SelectedObject;
            if (selected != null)
            {
                EditorState.Instance.Objects.Remove(selected);
                EditorState.Instance.SelectedObject = null;
                viewportMain.Invalidate();
            }
        }

        private void HandleObjectSelection(PointF worldPoint)
        {
            RectangleObject? hitObject = null;

            foreach (var obj in EditorState.Instance.Objects.AsEnumerable().Reverse())
            {
                if (obj.HitTest(worldPoint))
                {
                    hitObject = obj;
                    break;
                }
            }

            UpdateSelectionStates(hitObject);
            EditorState.Instance.SelectedObject = hitObject;

            if (hitObject != null)
            {
                if (hitObject.HitTestHandle(worldPoint, out int handleIndex))
                {
                    hitObject.SetActiveHandle(handleIndex);
                    _startPoint = worldPoint;
                    _isResizing = true;
                }
                else
                {
                    _dragStartPoint = worldPoint;
                    _isDraggingObject = true;
                }
            }
            else
            {
                _startPoint = worldPoint;
                _isDrawing = true;
            }
        }

        private void UpdateSelectionStates(RectangleObject? selected)
        {
            foreach (var obj in EditorState.Instance.Objects)
                obj.IsSelected = (obj == selected);
        }

        private void UpdateDrawingRectangle(PointF currentPoint)
        {
            var width = currentPoint.X - _startPoint.X;
            var height = currentPoint.Y - _startPoint.Y;
            _drawingRect = new RectangleF(_startPoint.X, _startPoint.Y, width, height);
            viewportMain.Invalidate();
        }

        private void DragSelectedObject(PointF currentPoint)
        {
            var offset = new PointF(
                currentPoint.X - _dragStartPoint.X,
                currentPoint.Y - _dragStartPoint.Y
            );

            EditorState.Instance.SelectedObject?.Move(offset);
            _dragStartPoint = currentPoint;
            viewportMain.Invalidate();
        }

        private void FinalizeDrawing()
        {
            _isDrawing = false;

            if (_drawingRect.Width != 0 && _drawingRect.Height != 0)
            {
                var newObj = new RectangleObject(_drawingRect);
                EditorState.Instance.Objects.Add(newObj);
            }

            _drawingRect = Rectangle.Empty;
        }
    }
}
