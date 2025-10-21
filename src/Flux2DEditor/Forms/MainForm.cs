using Flux2DEditor.Core;
using Flux2DEditor.Core.Models;
using Flux2DEditor.Render;
using System.Diagnostics;

namespace Flux2DEditor.Forms
{
    /// <summary>
    /// Main application form responsible for user interaction and rendering.
    /// Coordinates between the EditorState (model) and the viewport (view).
    /// </summary>
    public partial class MainForm : Form
    {
        #region Fields

        private readonly EditorState _editorState = new EditorState();
        private readonly EditorController _controller;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the main form and its dependencies.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            _controller = new EditorController(_editorState, new ShapeRenderer());
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles key commands like Delete for removing selected shapes.
        /// </summary>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            _controller.OnKeyDown(e.KeyCode, e.Control);
            viewportMain.Invalidate();
        }

        /// <summary>
        /// Handles mouse down events for selecting, dragging, resizing, or starting a new shape.
        /// </summary>
        private void viewportMain_MouseDown(object sender, MouseEventArgs e)
        {
            _controller.OnMouseDown(viewportMain.ScreenToWorld(e.Location), e.Button);
            viewportMain.Invalidate();
        }

        /// <summary>
        /// Handles mouse move events for updating dragging, resizing, or drawing operations.
        /// </summary>
        private void viewportMain_MouseMove(object sender, MouseEventArgs e)
        {
            _controller.OnMouseMove(viewportMain.ScreenToWorld(e.Location));
            viewportMain.Invalidate();
        }

        /// <summary>
        /// Handles mouse up events to finalize dragging, resizing, or drawing operations.
        /// </summary>
        private void viewportMain_MouseUp(object sender, MouseEventArgs e)
        {
            _controller.OnMouseUp(viewportMain.ScreenToWorld(e.Location));
            viewportMain.Invalidate();
        }

        /// <summary>
        /// Handles the rendering of shapes and preview rectangle in the viewport.
        /// </summary>
        private void viewportMain_Render(object sender, PaintEventArgs e)
        {
            _controller.Render(e.Graphics);
        }

        /// <summary>
        /// Handles shape selection from the toolbar and toggles the active drawing tool.
        /// </summary>
        private void toolStripShapeSelector_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == toolStripButtonRectangle)
            {
                _controller.ToggleShapeType("Rectangle");
                toolStripButtonRectangle.CheckState = _controller.CurrentShapeType == "Rectangle" ? CheckState.Checked : CheckState.Unchecked;
                toolStripButtonCircle.CheckState = CheckState.Unchecked;
            }
            else if (e.ClickedItem == toolStripButtonCircle)
            {
                _controller.ToggleShapeType("Circle");
                toolStripButtonCircle.CheckState = _controller.CurrentShapeType == "Circle" ? CheckState.Checked : CheckState.Unchecked;
                toolStripButtonRectangle.CheckState = CheckState.Unchecked;
            }
        }

        #endregion
    }
}
