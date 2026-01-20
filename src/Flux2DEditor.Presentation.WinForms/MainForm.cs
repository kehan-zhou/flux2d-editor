using Flux2DEditor.Application.Commands;
using Flux2DEditor.Application.Editor;
using Flux2DEditor.Application.HitTesting;
using Flux2DEditor.Application.Selection;
using Flux2DEditor.Application.Tools;
using Flux2DEditor.Domain.Geometry;
using Flux2DEditor.Domain.Scene;
using Flux2DEditor.Domain.Shapes;
using Flux2DEditor.Presentation.WinForms.Input;
using Flux2DEditor.Presentation.WinForms.Rendering;

namespace Flux2DEditor.Presentation.WinForms
{
    public partial class MainForm : Form
    {
        private readonly GdiSceneRenderer _renderer;
        private readonly EditorInputController _inputController;

        public MainForm()
        {
            InitializeComponent();

            KeyPreview = true;

            var scene = new Scene();

            var rect1 = new Domain.Shapes.Rectangle(ShapeId.New(), new Vector2(100, 100), new Vector2(120, 80));
            scene.Add(rect1);

            var rect2 = new Domain.Shapes.Rectangle(ShapeId.New(), new Vector2(100, 0), new Vector2(120, 80));
            scene.Add(rect2);

            var line1 = new Domain.Shapes.LineSegment(ShapeId.New(), new Vector2(150, 150), new Vector2(150, 200));
            scene.Add(line1);

            var selection = new SelectionService();
            var history = new CommandHistory();
            var editor = new EditorController(scene, selection, history);

            editor.RequestRedraw += OnRequestRedraw;

            var hitTest = new HitTestService();

            editor.SetActiveTool(new SelectTool(selection));

            _inputController = new EditorInputController(editor, hitTest, scene);

            _ = new WinFormsMouseAdapter(_inputController, viewport1);

            _renderer = new GdiSceneRenderer(scene, selection, editor);
        }

        private void viewport1_Render(object? sender, PaintEventArgs e)
        {
            _renderer.Render(e.Graphics);
        }

        private void OnRequestRedraw()
        {
            viewport1.Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Control && e.KeyCode == Keys.Z)
            {
                _inputController.Undo();
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                _inputController.Redo();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                _inputController.Cancel();
                e.Handled = true;
            }
        }
    }
}
