using Flux2DEditor.Application.Editor;
using Flux2DEditor.Application.Selection;
using Flux2DEditor.Application.Tools;
using Flux2DEditor.Domain.Geometry;
using Flux2DEditor.Domain.Scene;
using Flux2DEditor.Domain.Shapes;
using System.Drawing.Drawing2D;

namespace Flux2DEditor.Presentation.WinForms.Rendering
{
    public sealed class GdiSceneRenderer
    {
        private readonly Scene _scene;
        private readonly ISelectionService _selection;
        private readonly EditorController _editor;

        public GdiSceneRenderer(Scene scene, ISelectionService selection, EditorController editor)
        {
            _scene = scene;
            _selection = selection;
            _editor = editor;
        }

        public void Render(Graphics g)
        {
            var move = _editor.CurrentMove;
            var resize = _editor.CurrentResize;

            foreach (var shape in _scene.Shapes)
            {
                var renderShape = ResolveRenderShape(shape, move, resize);

                DrawShape(g, renderShape);

                if (_selection.IsSelected(shape.Id))
                {
                    DrawSelection(g, renderShape);
                    DrawHandles(g, renderShape);
                }
            }

            DrawSelectionBox(g);
            DrawCopyPreview(g, move);
        }

        private static Shape ResolveRenderShape(Shape shape, MoveContext? move, ResizeContext? resize)
        {
            if (resize != null && resize.ShapeId == shape.Id && resize.PreviewShape != null)
            {
                return resize.PreviewShape;
            }

            if (move != null && !move.IsCopy && move.ShapeIds.Contains(shape.Id))
            {
                return shape.Translate(move.CurrentDelta);
            }

            return shape;
        }

        private static void DrawShape(Graphics g, Shape shape)
        {
            switch (shape)
            {
                case Domain.Shapes.Rectangle rect:
                    DrawRectangle(g, rect);
                    break;

                case LineSegment line:
                    DrawLine(g, line);
                    break;
            }
        }

        private static void DrawRectangle(Graphics g, Domain.Shapes.Rectangle rect)
        {
            var pos = rect.Position;
            var size = rect.Size;

            g.FillRectangle(
                Brushes.LightGray,
                (float)pos.X,
                (float)pos.Y,
                (float)size.X,
                (float)size.Y);

            g.DrawRectangle(
                Pens.Black,
                (float)pos.X,
                (float)pos.Y,
                (float)size.X,
                (float)size.Y);
        }

        private static void DrawLine(Graphics g, LineSegment line)
        {
            g.DrawLine(
                Pens.Black,
                (float)line.Start.X,
                (float)line.Start.Y,
                (float)line.End.X,
                (float)line.End.Y);
        }

        private static void DrawSelection(Graphics g, Shape shape)
        {
            var box = shape.GetBoundingBox();

            if (shape is LineSegment)
            {
                box = InflateZeroSizedBox(box, 6);
            }

            using var pen = new Pen(Color.DeepSkyBlue)
            {
                DashStyle = DashStyle.Dash
            };

            g.DrawRectangle(
                pen,
                (float)box.Min.X,
                (float)box.Min.Y,
                (float)box.Width,
                (float)box.Height);
        }

        private static void DrawHandles(Graphics g, Shape shape)
        {
            if (shape is not IHandleProvider provider)
                return;

            const float size = 6f;
            const float half = size / 2f;

            foreach (var handle in provider.GetHandles())
            {
                var x = (float)handle.Position.X - half;
                var y = (float)handle.Position.Y - half;

                g.FillRectangle(Brushes.White, x, y, size, size);
                g.DrawRectangle(Pens.DeepSkyBlue, x, y, size, size);
            }
        }

        private static BoundingBox InflateZeroSizedBox(BoundingBox box, float minSize)
        {
            var min = box.Min;
            var max = box.Max;

            if (box.Width == 0)
            {
                min = new Vector2(min.X - minSize / 2, min.Y);
                max = new Vector2(max.X + minSize / 2, max.Y);
            }

            if (box.Height == 0)
            {
                min = new Vector2(min.X, min.Y - minSize / 2);
                max = new Vector2(max.X, max.Y + minSize / 2);
            }

            return new BoundingBox(min, max);
        }

        private void DrawSelectionBox(Graphics g)
        {
            if (_editor.ActiveTool is not SelectTool selectTool)
                return;

            var box = selectTool.CurrentBox;
            if (box == null) return;

            using var pen = new Pen(Color.DeepSkyBlue)
            {
                DashStyle = DashStyle.Dash
            };

            g.DrawRectangle(
                pen,
                (float)box.Value.Min.X,
                (float)box.Value.Min.Y,
                (float)box.Value.Width,
                (float)box.Value.Height);
        }

        private void DrawCopyPreview(Graphics g, MoveContext? move)
        {
            if (move == null || !move.IsCopy || move.PreviewCopies == null)
                return;

            foreach (var copy in move.PreviewCopies)
            {
                DrawShape(g, copy.Translate(move.CurrentDelta));
            }
        }
    }
}
