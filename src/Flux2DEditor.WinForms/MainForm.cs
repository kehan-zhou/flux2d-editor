using Flux2DEditor.Core.Editor;
using Flux2DEditor.Core.Geometry;
using Flux2DEditor.Core.Models;
using Flux2DEditor.Core.Rendering;
using Flux2DEditor.Render.Gdi;
using System.Drawing.Drawing2D;
using System.Numerics;

namespace Flux2DEditor.WinForms
{
    /// <summary>
    /// Main application form responsible for user interaction and rendering.
    /// Coordinates between the EditorState (model) and the viewport (view).
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly EditorState _editor = new EditorState();

        private enum ToolType
        {
            Select, 
            Rectangle, 
            Circle
        }

        private ToolType _currentTool = ToolType.Select;

        private bool _isDrawingShape = false;
        private bool _isDragging = false;
        private bool _isResizing = false;
        private int _activeHandleIndex = -1;

        private BoundingBox _resizeStartBounds;
        private Vector2 _lastWorldPos;
        private Vector2 _drawStartPos;
        private RectangleF _drawPreviewRect;

        public MainForm()
        {
            InitializeComponent();
        }

        #region Event Handlers

        /// <summary>
        /// Handles key commands like Delete for removing selected shapes.
        /// </summary>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                _editor.DeleteSelectedShape();
                viewportMain.Invalidate();
            }

            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C: _editor.CopySelectedShape(); break;
                    case Keys.X: _editor.CutSelectedShape(); break;
                    case Keys.V: _editor.PasteShape(); break;
                    case Keys.Z: _editor.Commands.Undo(); break;
                    case Keys.Y: _editor.Commands.Redo(); break;
                }

                viewportMain.Invalidate();
            }
        }

        /// <summary>
        /// Handles mouse down events for selecting, dragging, resizing, or starting a new shape.
        /// </summary>
        private void viewportMain_MouseDown(object sender, MouseEventArgs e)
        {
            var wp = viewportMain.ScreenToWorld(e.Location);
            var wv = new Vector2(wp.X, wp.Y);

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            switch (_currentTool)
            {
                case ToolType.Rectangle:
                case ToolType.Circle:
                    {
                        _isDrawingShape = true;
                        _drawStartPos = wv;
                        _drawPreviewRect = RectangleF.Empty;
                        return;
                    }

                case ToolType.Select:
                default:
                    {
                        var hit = _editor.FindShapeAt(wv);

                        if (hit is BaseShape bs && bs.HitTestHandle(wv, out _activeHandleIndex))
                        {
                            _editor.SelectShape(hit);

                            _isResizing = true;
                            _resizeStartBounds = bs.Bounds;

                            viewportMain.Invalidate();
                            return;
                        }

                        _editor.SelectShape(hit);

                        if (hit != null)
                        {
                            _isDragging = true;
                            _lastWorldPos = wv;
                        }

                        viewportMain.Invalidate();
                        return;
                    }       
            }   
        }

        /// <summary>
        /// Handles mouse move events for updating dragging, resizing, or drawing operations.
        /// </summary>
        private void viewportMain_MouseMove(object sender, MouseEventArgs e)
        {
            var wp = viewportMain.ScreenToWorld(e.Location);
            var wv = new Vector2(wp.X, wp.Y);

            if (_isDrawingShape)
            {
                float x = Math.Min(_drawStartPos.X, wv.X);
                float y = Math.Min(_drawStartPos.Y, wv.Y);
                float w = Math.Abs(wv.X - _drawStartPos.X);
                float h = Math.Abs(wv.Y - _drawStartPos.Y);

                if (_currentTool == ToolType.Circle)
                {
                    float size = Math.Min(w, h);
                    w = h = size;

                    x = _drawStartPos.X < wv.X ? _drawStartPos.X : _drawStartPos.X - size;
                    y = _drawStartPos.Y < wv.Y ? _drawStartPos.Y : _drawStartPos.Y - size;
                }

                _drawPreviewRect = new RectangleF(x, y, w, h);

                viewportMain.Invalidate();
                return;
            }
           
            if (_isResizing && _editor.SelectedShape != null)
            {
                _editor.SelectedShape.ResizeFromHandle(_activeHandleIndex, wv);

                viewportMain.Invalidate();
                return;
            }

            if (_isDragging && _editor.SelectedShape != null)
            {
                var delta = wv - _lastWorldPos;
                _editor.MoveSelectedShape(delta);
                    
                _lastWorldPos = wv;
                viewportMain.Invalidate();
                return;
            }
            
        }

        /// <summary>
        /// Handles mouse up events to finalize dragging, resizing, or drawing operations.
        /// </summary>
        private void viewportMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            if (_isDrawingShape)
            {
                _isDrawingShape = false;

                if (_drawPreviewRect.Width > 1 && _drawPreviewRect.Height > 1)
                {
                    BoundingBox box = new BoundingBox(
                        _drawPreviewRect.X,
                        _drawPreviewRect.Y,
                        _drawPreviewRect.Width,
                        _drawPreviewRect.Height
                    );

                    if (_currentTool == ToolType.Rectangle)
                    {
                        _editor.AddShape(new RectangleShape(box));
                    }
                    else if (_currentTool == ToolType.Circle)
                    {
                        _editor.AddShape(new CircleShape(box));
                    }
                }

                _drawPreviewRect = RectangleF.Empty;
                viewportMain.Invalidate();
                return;
            }

            if (_isDragging)
            {
                _isDragging = false;
            }

            if (_isResizing)
            {
                _isResizing = false;

                if (_editor.SelectedShape != null)
                {
                    var newBounds = _editor.SelectedShape.Bounds;
                    _editor.ResizeSelectedShape(_resizeStartBounds, newBounds);
                }

                _activeHandleIndex = -1;
            }
        }

        /// <summary>
        /// Handles the rendering of shapes and preview rectangle in the viewport.
        /// </summary>
        private void viewportMain_Render(object sender, PaintEventArgs e)
        {
            using var renderer = new GdiRenderer(e.Graphics);
            var ctx = renderer.BeginDraw();

            foreach (var shape in _editor.Shapes)
            {
                shape.Draw(ctx, RenderStyles.Default);
            }

            if (_isDrawingShape && _drawPreviewRect != RectangleF.Empty)
            {
                using var previewPen = new Pen(Color.Red, 1f)
                {
                    DashStyle = DashStyle.Dash
                };

                e.Graphics.DrawRectangle(
                    previewPen,
                    _drawPreviewRect.X,
                    _drawPreviewRect.Y,
                    _drawPreviewRect.Width,
                    _drawPreviewRect.Height
                );
            }

            renderer.EndDraw();

        }

        /// <summary>
        /// Handles shape selection from the toolbar and toggles the active drawing tool.
        /// </summary>
        private void toolStripShapeSelector_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            toolStripButtonSelect.CheckState = CheckState.Unchecked;
            toolStripButtonRectangle.CheckState = CheckState.Unchecked;
            toolStripButtonCircle.CheckState = CheckState.Unchecked;

            if (e.ClickedItem == toolStripButtonSelect)
            {
                _currentTool = ToolType.Select;
                toolStripButtonSelect.CheckState = CheckState.Checked;
            }
            else if (e.ClickedItem == toolStripButtonRectangle)
            {
                _currentTool = ToolType.Rectangle;
                toolStripButtonRectangle.CheckState = CheckState.Checked;
            }
            else if (e.ClickedItem == toolStripButtonCircle)
            {
                _currentTool = ToolType.Circle;
                toolStripButtonCircle.CheckState = CheckState.Checked;
            }
        }

        #endregion
    }
}
