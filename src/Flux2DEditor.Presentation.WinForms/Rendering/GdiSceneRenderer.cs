using Flux2DEditor.Application.Editor;
using Flux2DEditor.Application.Selection;
using Flux2DEditor.Application.Tools;
using Flux2DEditor.Domain.Geometry;
using Flux2DEditor.Domain.Scene;
using Flux2DEditor.Domain.Shapes;
using System.Drawing;
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

            foreach (var shape in _scene.Shapes)
            {
                if (move != null && !move.IsCopy && move.ShapeIds.Contains(shape.Id))
                {
                    DrawShapeWithOffset(g, shape, move.CurrentDelta);
                }
                else
                {
                    DrawShape(g, shape);
                }

                if (_selection.IsSelected(shape.Id))
                {
                    DrawSelection(g, shape, move);
                }
            }

            if (_editor.ActiveTool is SelectTool selectTool)
            {
                var box = selectTool.CurrentBox;
                if (box != null)
                {
                    using var pen = new Pen(Color.DeepSkyBlue)
                    {
                        DashStyle = DashStyle.Dash
                    };

                    g.DrawRectangle(pen, (float)box.Value.Min.X, (float)box.Value.Min.Y, (float)box.Value.Width, (float)box.Value.Height);
                }
            }

            if (move != null && move.IsCopy && move.PreviewCopies != null)
            {
                foreach (var copy in move.PreviewCopies)
                {
                    DrawShapeWithOffset(g, copy, move.CurrentDelta);
                }
            }
        }

        private static void DrawShape(Graphics g, Shape shape)
        {
            if (shape is Domain.Shapes.Rectangle rect)
            {
                var pos = rect.Position;
                var size = rect.Size;

                g.FillRectangle(Brushes.LightGray, (float)pos.X, (float)pos.Y, (float)size.X, (float)size.Y);

                g.DrawRectangle(Pens.Black, (float)pos.X, (float)pos.Y, (float)size.X, (float)size.Y);
            }
            else if (shape is Domain.Shapes.LineSegment line)
            {
                g.DrawLine(
                    Pens.Black,
                    (float)line.Start.X,
                    (float)line.Start.Y,
                    (float)line.End.X,
                    (float)line.End.Y
                );
            }
        }

        private static void DrawShapeWithOffset(Graphics g, Shape shape, Vector2 delta)
        {
            if (shape is Domain.Shapes.Rectangle rect)
            {
                g.FillRectangle(Brushes.LightGray, (float)(rect.Position.X + delta.X), (float)(rect.Position.Y + delta.Y), (float)rect.Size.X, (float)rect.Size.Y);

                g.DrawRectangle(Pens.Black, (float)(rect.Position.X + delta.X), (float)(rect.Position.Y + delta.Y), (float)rect.Size.X, (float)rect.Size.Y);
            }
            else if (shape is LineSegment line)
            {
                g.DrawLine(Pens.Black, (float)(line.Start.X + delta.X), (float)(line.Start.Y + delta.Y), (float)(line.End.X + delta.X), (float)(line.End.Y + delta.Y));
            }
        }

        private static void DrawSelection(Graphics g, Shape shape, MoveContext? move)
        {
            var box = shape.GetBoundingBox();

            if (move != null && move.ShapeIds.Contains(shape.Id))
            {
                box = box.Translate(move.CurrentDelta);
            }

            if (shape is LineSegment)
            {
                const float minSize = 6f;

                if (box.Width == 0)
                {
                    box = new BoundingBox(
                        new Vector2(box.Min.X - minSize / 2, box.Min.Y),
                        new Vector2(box.Min.X + minSize / 2, box.Max.Y));
                }

                if (box.Height == 0)
                {
                    box = new BoundingBox(
                        new Vector2(box.Min.X, box.Min.Y - minSize / 2),
                        new Vector2(box.Max.X, box.Min.Y + minSize / 2));
                }
            }

            using var pen = new Pen(Color.DeepSkyBlue) { DashStyle = DashStyle.Dash };

            g.DrawRectangle(
                pen,
                (float)box.Min.X,
                (float)box.Min.Y,
                (float)box.Width,
                (float)box.Height);
        }
    }
}
