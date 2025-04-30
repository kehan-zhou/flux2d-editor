using Flux2DEditor.Core;

namespace Flux2DEditor.Forms
{
    public partial class MainForm : Form
    {
        private PointF _startPoint = PointF.Empty;
        private PointF _dragStartPoint = PointF.Empty;
        private RectangleF _currentRectangle = Rectangle.Empty;
        private bool _isDrawing = false;
        private bool _isDraggingObject = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void viewportMain_MouseDown(object sender, MouseEventArgs e)
        {
            var worldPoint = viewportMain.ScreenToWorld(e.Location);

            if (e.Button == MouseButtons.Left)
            {
                var selected = EditorState.Instance.SelectedObject;
                if (selected != null && selected.HitTest(worldPoint))
                {
                    _isDraggingObject = true;
                    _dragStartPoint = worldPoint;
                    return;
                }

                RectangleObject? hitObject = null;
                foreach (var obj in EditorState.Instance.Objects.AsEnumerable().Reverse())
                {
                    if (obj.HitTest(worldPoint))
                    {
                        hitObject = obj;
                        break;
                    }
                }

                foreach (var obj in EditorState.Instance.Objects)
                {
                    obj.IsSelected = (obj == hitObject);
                }

                EditorState.Instance.SelectedObject = hitObject;

                if (hitObject == null)
                {
                    _startPoint = worldPoint;
                    _isDrawing = true;
                }

                viewportMain.Invalidate();
            }
        }

        private void viewportMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                var currentPoint = viewportMain.ScreenToWorld(e.Location);

                var width = currentPoint.X - _startPoint.X;
                var height = currentPoint.Y - _startPoint.Y;
                _currentRectangle = new RectangleF(_startPoint.X, _startPoint.Y, width, height);
                viewportMain.Invalidate();
            }

            if (_isDraggingObject)
            {
                var currentPoint = viewportMain.ScreenToWorld(e.Location);
                var offset = new PointF(currentPoint.X - _dragStartPoint.X, currentPoint.Y - _dragStartPoint.Y);

                EditorState.Instance.SelectedObject?.Move(offset);
                _dragStartPoint = currentPoint;

                viewportMain.Invalidate();
            }
        }

        private void viewportMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                _isDrawing = false;

                if(_currentRectangle.Width != 0 && _currentRectangle.Height != 0)
                {
                    var newObj = new RectangleObject(_currentRectangle);
                    EditorState.Instance.Objects.Add(newObj);
                } 

                _currentRectangle = Rectangle.Empty;
                viewportMain.Invalidate();
            }

            if (_isDraggingObject)
            {
                _isDraggingObject = false;
            }
        }

        private void viewportMain_Render(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            foreach (var obj in EditorState.Instance.Objects)
            {
                obj.Draw(g);
            }

            if (_currentRectangle != Rectangle.Empty)
            {
                using var redPen = new Pen(Color.Red, 2);
                g.DrawRectangle(redPen, _currentRectangle);
            }
        }
    }
}
