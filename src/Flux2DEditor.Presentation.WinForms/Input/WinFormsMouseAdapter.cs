using Flux2DEditor.Domain.Geometry;
using ViewportControl;

namespace Flux2DEditor.Presentation.WinForms.Input
{
    public sealed class WinFormsMouseAdapter
    {
        private readonly EditorInputController _input;
        private readonly Viewport _viewport;

        public WinFormsMouseAdapter(EditorInputController input, Viewport viewport)
        {
            _input = input;
            _viewport = viewport;

            _viewport.MouseDown += OnMouseDown;
            _viewport.MouseMove += OnMouseMove;
            _viewport.MouseUp += OnMouseUp;
        }

        private void OnMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            _input.OnPointerDown(ToWorld(e.Location));
        }

        private void OnMouseMove(object? sender, MouseEventArgs e)
        {
            _input.OnPointerMove(ToWorld(e.Location));
        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            _input.OnPointerUp(ToWorld(e.Location));
        }

        private Vector2 ToWorld(Point point)
        {
            var worldPoint = _viewport.ScreenToWorld(point);
            return new Vector2(worldPoint.X, worldPoint.Y);
        }
    }
}
