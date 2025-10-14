using Flux2DEditor.Core.Interfaces;
using Flux2DEditor.Core.Models;

namespace Flux2DEditor.Core
{
    /// <summary>
    /// Centralized controller for handling editor interactions.
    /// Encapsulates mouse, keyboard, and toolbar operations.
    /// </summary>
    public class EditorController
    {
        #region Fields
        
        private readonly EditorState _state;
        private readonly IShapeRenderer _renderer;

        #endregion

        #region Interaction State

        private PointF _startPoint = PointF.Empty;
        private PointF _dragStartPoint = PointF.Empty;
        private RectangleF _previewRectangle = RectangleF.Empty;

        private bool _isDrawing = false;
        private bool _isDragging = false;
        private bool _isResizing = false;
        private int _activeHandleIndex = -1;

        private string _selectedShapeType = string.Empty;

        #endregion

        #region Constructor

        public EditorController(EditorState state, IShapeRenderer renderer)
        {
            _state = state;
            _renderer = renderer;
        }

        #endregion

        #region Shape Type Selection (Toolbar)

        public void ToggleShapeType(string type)
        {
            if (_selectedShapeType.Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                _selectedShapeType = string.Empty; // Deselect if already selected
            }
            else
            {
                _selectedShapeType = type;
            }
        }

        public string CurrentShapeType => _selectedShapeType;

        #endregion

        #region Mouse Events

        public void OnMouseDown(PointF worldPoint, MouseButtons button)
        {
            if (button != MouseButtons.Left) return;

            var hitShape = _state.FindShapeAt(worldPoint);

            if (string.IsNullOrEmpty(_selectedShapeType))
            {
                // Resize
                if (hitShape is BaseShape shape && shape.HitTestHandle(worldPoint, out int handleIndex))
                {
                    _isResizing = true;
                    _activeHandleIndex = handleIndex;
                }
                // Drag
                else if (hitShape != null)
                {
                    _isDragging = true;
                    _dragStartPoint = worldPoint;
                }

                _state.SelectShape(hitShape);
            }
            else
            {
                if (hitShape == null)
                {
                    _isDrawing = true;
                    _startPoint = worldPoint;
                }
            }
        }

        public void OnMouseMove(PointF worldPoint)
        {
            // Drag
            if (_isDragging && _state.SelectedShape != null)
            {
                var offset = new PointF(worldPoint.X - _dragStartPoint.X, worldPoint.Y - _dragStartPoint.Y);
                _state.SelectedShape.Move(offset);
                _dragStartPoint = worldPoint;
            }

            // Resize
            if (_isResizing && _state.SelectedShape is BaseShape shape)
            {
                shape.ResizeFromHandle(_activeHandleIndex, worldPoint);
            }

            // Draw preview
            if (_isDrawing)
            {
                var x = Math.Min(_startPoint.X, worldPoint.X);
                var y = Math.Min(_startPoint.Y, worldPoint.Y);
                var width = Math.Abs(worldPoint.X - _startPoint.X);
                var height = Math.Abs(worldPoint.Y - _startPoint.Y);
                _previewRectangle = new RectangleF(x, y, width, height);
            }
        }

        public void OnMouseUp(PointF worldPoint)
        {
            if (_isDragging) _isDragging = false;
            if (_isResizing)
            {
                _isResizing = false;
                _activeHandleIndex = -1;
            }

            if (_isDrawing)
            {
                _isDrawing = false;
                if (_previewRectangle.Width > 0 && _previewRectangle.Height > 0)
                {
                    var newShape = ShapeFactory.CreateShape(_selectedShapeType, _previewRectangle);
                    _state.AddShape(newShape);
                }
                _previewRectangle = RectangleF.Empty;
            }
        }

        #endregion

        #region Keyboard / Delete

        public void OnKeyDown(Keys key)
        {
            if (key == Keys.Delete)
            {
                _state.DeleteSelectedShape();
            }
        }

        #endregion

        #region Rendering

        public void Render(Graphics g)
        {
            foreach (var shape in _state.Shapes)
            {
                _renderer.Render(shape, g);
            }


            if (_previewRectangle != RectangleF.Empty)
            {
                using var pen = new Pen(Color.Red, 2f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
                g.DrawRectangle(pen, _previewRectangle.X, _previewRectangle.Y, _previewRectangle.Width, _previewRectangle.Height);
            }
        }
        
        #endregion
    }
}