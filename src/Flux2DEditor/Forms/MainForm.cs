using Flux2DEditor.Core;
using Flux2DEditor.Core.Shapes;
using System.Diagnostics;

namespace Flux2DEditor.Forms
{
    public partial class MainForm : Form
    {
        private PointF _startPoint = PointF.Empty;
        private PointF _dragStartPoint = PointF.Empty;
        private RectangleF _previewRectangle = RectangleF.Empty;

        private bool _isDrawing = false;
        private bool _isDragging = false;
        private bool _isResizing = false;
        
        private int _activeHandleIndex = -1;
        
        private string _selectedShapeType = "Rectangle";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                EditorState.Instance.DeleteSelectedShape();
                viewportMain.Invalidate();
            }
        }

        private void viewportMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var worldPoint = viewportMain.ScreenToWorld(e.Location);
            var hitShape = EditorState.Instance.FindShapeAt(worldPoint);
            EditorState.Instance.SelectShape(hitShape);

            if (hitShape is RectangleShape rectShape && rectShape.HitTestHandle(worldPoint, out int handleIndex))
            {
                _isResizing = true;
                _activeHandleIndex = handleIndex;
            }
            else if (hitShape != null)
            {
                _isDragging = true;
                _dragStartPoint = worldPoint;
            }
            else
            {
                _isDrawing = true;
                _startPoint = worldPoint;
            }

            viewportMain.Invalidate();
        }

        private void viewportMain_MouseMove(object sender, MouseEventArgs e)
        {
            var worldPoint = viewportMain.ScreenToWorld(e.Location);

            if (_isDragging && EditorState.Instance.SelectedShape != null)
            {
                var offset = new PointF(worldPoint.X - _dragStartPoint.X, worldPoint.Y - _dragStartPoint.Y);
                EditorState.Instance.SelectedShape.Move(offset);
                _dragStartPoint = worldPoint;
                viewportMain.Invalidate();
            }

            if (_isResizing && EditorState.Instance.SelectedShape is RectangleShape rectShape)
            {
                rectShape.ResizeFromHandle(_activeHandleIndex, worldPoint);
                viewportMain.Invalidate();
            }

            if (_isDrawing)
            {
                _previewRectangle = new RectangleF(
                    Math.Min(_startPoint.X, worldPoint.X),
                    Math.Min(_startPoint.Y, worldPoint.Y),
                    Math.Abs(worldPoint.X - _startPoint.X),
                    Math.Abs(worldPoint.Y - _startPoint.Y)
                );

                viewportMain.Invalidate();
            }
        }

        private void viewportMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
            }

            if (_isResizing)
            {
                _isResizing = false;
                _activeHandleIndex = -1;
            }

            if (_isDrawing)
            {
                _isDrawing = false;
                if (_previewRectangle.Width > 0 && _previewRectangle.Height > 0)
                {
                    var newShape = ShapeFactory.CreateShape(_selectedShapeType, _previewRectangle);
                    EditorState.Instance.AddShape(newShape);
                }

                _previewRectangle = RectangleF.Empty;
            }

            viewportMain.Invalidate();
        }

        private void viewportMain_Render(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            foreach (var shape in EditorState.Instance.Shapes)
            {
                shape.Draw(g);
            }

            if (_previewRectangle != Rectangle.Empty)
            {
                using var pen = new Pen(Color.Red, 2);
                g.DrawRectangle(pen, _previewRectangle.X, _previewRectangle.Y, _previewRectangle.Width, _previewRectangle.Height);
            }
        }
    }
}
