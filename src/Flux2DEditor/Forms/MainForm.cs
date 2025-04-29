using Flux2DEditor.Core;

namespace Flux2DEditor.Forms
{
    public partial class MainForm : Form
    {
        private PointF _startPoint = PointF.Empty;
        private RectangleF _currentRectangle = Rectangle.Empty;
        private bool _isDrawing = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void viewportMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _startPoint = viewportMain.ScreenToWorld(e.Location);
                _isDrawing = true;
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
        }

        private void viewportMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                _isDrawing = false;

                if(_currentRectangle.Width != 0 && _currentRectangle.Height != 0)
                {
                    EditorState.Instance.Rectangles.Add(new EditorRectangle(_currentRectangle));
                } 

                _currentRectangle = Rectangle.Empty;
                viewportMain.Invalidate();
            }
        }

        private void viewportMain_Render(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            foreach (var rect in EditorState.Instance.Rectangles)
            {
                using var pen = new Pen(rect.Color, 2);
                g.DrawRectangle(pen, rect.Bounds);
            }

            if (_currentRectangle != Rectangle.Empty)
            {
                using var redPen = new Pen(Color.Red, 2);
                g.DrawRectangle(redPen, _currentRectangle);
            }
        }
    }
}
