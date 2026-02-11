using Flux2DEditor.Application.Editor;
using Flux2DEditor.Application.HitTesting;
using Flux2DEditor.Application.Tools;
using Flux2DEditor.Domain.Geometry;
using Flux2DEditor.Domain.Scene;

namespace Flux2DEditor.Presentation.WinForms.Input
{
    public sealed class EditorInputController
    {
        private const double DragThresholdSquared = 4.0;

        private readonly EditorController _editor;
        private readonly HitTestService _hitTestService;
        private readonly Scene _scene;

        private bool _isDragging;
        private bool _pendingClick;
        private bool _pendingMove;
        private bool _pendingBoxSelect;
        private bool _pendingCopy;

        private Vector2 _pointerDownPosition;

        public EditorInputController(EditorController editor, HitTestService hitTestService, Scene scene)
        {
            _editor = editor;
            _hitTestService = hitTestService;
            _scene = scene;
        }

        private Application.Selection.SelectionMode GetSelectionMode()
        {
            if ((Control.ModifierKeys & Keys.Control) != 0)
            {
                return Application.Selection.SelectionMode.Toggle;
            }

            if ((Control.ModifierKeys & Keys.Shift) != 0)
            {
                return Application.Selection.SelectionMode.Add;
            }

            return Application.Selection.SelectionMode.Replace;
        }

        private bool IsCopyModifier()
        {
            return (Control.ModifierKeys & Keys.Control) != 0;
        }

        private bool IsAxisLockModifier()
        {
            return (Control.ModifierKeys & Keys.Shift) != 0;
        }

        public void OnPointerDown(Vector2 worldPosition)
        {
            _pointerDownPosition = worldPosition;
            _pendingClick = true;
            _pendingMove = false;
            _pendingBoxSelect = false;
            _isDragging = false;


            var hit = _hitTestService.HitTest(_scene, worldPosition);

            if (hit.IsHandle)
            {
                var shapeId = hit.HandleShapeId!.Value;

                if (!_editor.IsSelected(shapeId))
                {
                    _editor.SelectSingle(shapeId);
                }

                _editor.BeginResize(shapeId, hit.HandleType!.Value);
                _isDragging = true;
                _pendingClick = false;
                return;
            }

            if (_editor.ActiveTool is not SelectTool select)
                return;

            if (hit.HitShapeId != null)
            {
                _pendingMove = true;
                _pendingCopy = IsCopyModifier();
            }
            else
            {
                _pendingBoxSelect = true;
            }
        }

        public void OnPointerMove(Vector2 worldPosition)
        {
            var deltaSq = (worldPosition - _pointerDownPosition).LengthSquared();

            if (!_isDragging && deltaSq > DragThresholdSquared)
            {
                _pendingClick = false;

                if (_pendingMove)
                {
                    _editor.BeginMove(_pointerDownPosition, _pendingCopy);
                    _isDragging = true;
                }
                else if (_pendingBoxSelect &&
                         _editor.ActiveTool is SelectTool select)
                {
                    select.BeginBoxSelect(_pointerDownPosition);
                    _isDragging = true;
                }

                _pendingMove = false;
                _pendingBoxSelect = false;
            }

            if (_editor.ActiveTool is SelectTool s && s.IsBoxSelecting)
            {
                s.UpdateBoxSelect(worldPosition);
                _editor.NotifyInteractionUpdated();
                return;
            }

            if (_isDragging && _editor.CurrentMove != null)
            {
                _editor.UpdateMove(worldPosition, IsAxisLockModifier());
                return;
            }

            if (_editor.CurrentResize != null)
            {
                _editor.UpdateResize(worldPosition);
            }
        }

        public void OnPointerUp(Vector2 worldPosition)
        {
            if (_editor.ActiveTool is SelectTool select && select.IsBoxSelecting)
            {
                select.EndBoxSelect(_scene, GetSelectionMode());
                _editor.NotifyInteractionUpdated();
                ResetState();
                return;
            }

            if (_pendingClick)
            {
                if (_editor.ActiveTool is SelectTool selectTool)
                {
                    var hit = _hitTestService.HitTest(_scene, worldPosition);
                    selectTool.SelectSingle(hit, GetSelectionMode());
                    _editor.NotifyInteractionUpdated();
                }

                ResetState();
                return;
            }

            if (_isDragging && _editor.CurrentMove != null)
            {
                _editor.EndMove(worldPosition);
                ResetState();
                return;
            }

            if (_editor.CurrentResize != null)
            {
                _editor.EndResize();
                ResetState();
                return;
            }

            ResetState();
        }

        public void Cancel()
        {
            ResetState();
            _editor.CancelInteraction();
        }

        private void ResetState()
        {
            _isDragging = false;
            _pendingClick = false;
            _pendingMove = false;
            _pendingBoxSelect = false;
            _pendingCopy = false;
        }

        public void Undo() => _editor.Undo();
        public void Redo() => _editor.Redo();
    }
}
