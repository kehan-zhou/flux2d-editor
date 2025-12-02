using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using Flux2DEditor.Core.Geometry;
using Flux2DEditor.Core.Rendering;

namespace Flux2DEditor.Render.Gdi
{
    /// <summary>
    /// GDI+ implementation of IRenderContext that maps abstract draw calls to System.Drawing.Graphics.
    /// </summary>
    public sealed class GdiRenderContext : IRenderContext
    {
        private readonly Graphics _g;

        public GdiRenderContext(Graphics g)
        {
            _g = g ?? throw new ArgumentNullException(nameof(g));

            _g.SmoothingMode = SmoothingMode.AntiAlias;
            _g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        }

        private static Color ToColor(Vector4 v)
        {
            float a = Math.Clamp(v.W, 0f, 1f);
            float r = Math.Clamp(v.X, 0f, 1f);
            float g = Math.Clamp(v.Y, 0f, 1f);
            float b = Math.Clamp(v.Z, 0f, 1f);
            return Color.FromArgb((int)MathF.Round(a * 255f), (int)MathF.Round(r * 255f), (int)MathF.Round(g * 255f), (int)MathF.Round(b * 255f));
        }

        private static Pen CreatePen(StrokeStyle stroke)
        {
            var color = ToColor(stroke.Color);
            var pen = new Pen(color, Math.Max(0.1f, stroke.Thickness));
            if (stroke.IsDashed)
            {
                pen.DashStyle = DashStyle.Dash;
            }
            else
            {
                pen.DashStyle = DashStyle.Solid;
            }
            pen.LineJoin = LineJoin.Round;
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            return pen;
        }

        private static Brush CreateBrush(FillStyle fill)
        {
            var c = ToColor(fill.Color);
            return new SolidBrush(c);
        }

        public void DrawLine(Vector2 a, Vector2 b, StrokeStyle stroke)
        {
            using var pen = CreatePen(stroke);
            _g.DrawLine(pen, a.X, a.Y, b.X, b.Y);
        }

        public void DrawRectangle(BoundingBox box, StrokeStyle stroke)
        {
            using var pen = CreatePen(stroke);
            var rect = new RectangleF(box.Min.X, box.Min.Y, box.Width, box.Height);
            _g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void FillRectangle(BoundingBox box, FillStyle fill)
        {
            if (fill.Color.W <= 0f) return;
            using var brush = CreateBrush(fill);
            var rect = new RectangleF(box.Min.X, box.Min.Y, box.Width, box.Height);
            _g.FillRectangle(brush, rect);
        }

        public void DrawCircle(Vector2 center, float radius, StrokeStyle stroke)
        {
            using var pen = CreatePen(stroke);
            var rect = new RectangleF(center.X - radius, center.Y - radius, radius * 2f, radius * 2f);
            _g.DrawEllipse(pen, rect);
        }

        public void FillCircle(Vector2 center, float radius, FillStyle fill)
        {
            if (fill.Color.W <= 0f) return;
            using var brush = CreateBrush(fill);
            var rect = new RectangleF(center.X - radius, center.Y - radius, radius * 2f, radius * 2f);
            _g.FillEllipse(brush, rect);
        }

        public void DrawPolyline(ReadOnlySpan<Vector2> points, bool closed, StrokeStyle stroke)
        {
            if (points.Length < 2) return;
            using var pen = CreatePen(stroke);

            var pts = ToPointFArray(points);
            if (closed)
            {
                _g.DrawPolygon(pen, pts);
            }
            else
            {
                _g.DrawLines(pen, pts);
            }
        }

        public void FillPolygon(ReadOnlySpan<Vector2> points, FillStyle fill)
        {
            if (points.Length < 3) return;
            if (fill.Color.W <= 0f) return;
            using var brush = CreateBrush(fill);
            var pts = ToPointFArray(points);
            _g.FillPolygon(brush, pts);
        }

        public void DrawText(string text, Vector2 position, Vector4 color, float size = 12f)
        {
            if (string.IsNullOrEmpty(text)) return;
            using var font = new Font(SystemFonts.DefaultFont.FontFamily, Math.Max(1f, size));
            using var brush = new SolidBrush(ToColor(color));
            _g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            _g.DrawString(text, font, brush, position.X, position.Y);
        }

        private static PointF[] ToPointFArray(ReadOnlySpan<Vector2> points)
        {
            var arr = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                arr[i] = new PointF(points[i].X, points[i].Y);
            }
            return arr;
        }
    }
}
