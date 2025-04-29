namespace Flux2DEditor.Forms
{
    public partial class Form1 : Form
    {
        private PointF _startPoint = PointF.Empty;
        private RectangleF _currentRectangle = Rectangle.Empty;
        private bool _isDrawing = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void viewport1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _startPoint = viewport1.ScreenToWorld(e.Location);
                _isDrawing = true;
            }
        }

        private void viewport1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                var currentPoint = viewport1.ScreenToWorld(e.Location);

                var width = currentPoint.X - _startPoint.X;
                var height = currentPoint.Y - _startPoint.Y;
                _currentRectangle = new RectangleF(_startPoint.X, _startPoint.Y, width, height);
                viewport1.Invalidate();
            }
        }

        private void viewport1_MouseUp(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                _isDrawing = false;
                viewport1.Invalidate();
            }
        }

        private void viewport1_Render(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            using var redPen = new Pen(Color.Red, 2);
            if (_currentRectangle != Rectangle.Empty)
            {
                g.DrawRectangle(redPen, _currentRectangle);
            }
        }
    }
}
