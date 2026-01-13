using Flux2DEditor.Application.Editor;
using Flux2DEditor.Application.HitTesting;
using Flux2DEditor.Application.Tools;
using Flux2DEditor.Domain.Geometry;
using Flux2DEditor.Domain.Scene;

namespace Flux2DEditor.Presentation.WinForms.Input
{
    public sealed class EditorInputController
    {
        private readonly EditorController _editor;
        private readonly HitTestService _hitTestService;
        private readonly Scene _scene;

        private bool _isDragging;
        private bool _pendingMove;
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

        public void OnPointerDown(Vector2 worldPosition)
        {
            var hit = _hitTestService.HitTest(_scene, worldPosition);

            if (_editor.ActiveTool is not SelectTool select)
                return;

            var mode = GetSelectionMode();

            if (hit.HitShapeId == null)
            {
                select.BeginBoxSelect(worldPosition);
                _editor.NotifyInteractionUpdated();
                return;
            }

            _pendingMove = true;
            _pendingCopy = IsCopyModifier();
            _pointerDownPosition = worldPosition;

            if (!_editor.SelectedShapeIds.Contains(hit.HitShapeId.Value))
            {
                select.SelectSingle(hit, Application.Selection.SelectionMode.Replace);
                _editor.NotifyInteractionUpdated();
            }
        }

        public void OnPointerMove(Vector2 worldPosition)
        {
            if (_editor.ActiveTool is SelectTool select && select.IsBoxSelecting)
            {
                select.UpdateBoxSelect(worldPosition);
                _editor.NotifyInteractionUpdated();
                return;
            }

            if (_pendingMove)
            {
                if ((worldPosition - _pointerDownPosition).LengthSquared() > 4)
                {
                    _editor.BeginMove(_pointerDownPosition, _pendingCopy);
                    _isDragging = true;
                    _pendingMove = false;
                }
            }

            if (_isDragging)
            {
                _editor.UpdateMove(worldPosition);
            }
        }

        public void OnPointerUp(Vector2 worldPosition)
        {
            if (_editor.ActiveTool is SelectTool select && select.IsBoxSelecting)
            {
                select.EndBoxSelect(_scene, GetSelectionMode());
                _editor.NotifyInteractionUpdated();
                return;
            }

            if (_pendingMove)
            {
                if (_editor.ActiveTool is SelectTool selectTool)
                {
                    var hit = _hitTestService.HitTest(_scene, worldPosition);
                    selectTool.SelectSingle(hit, GetSelectionMode());
                    _editor.NotifyInteractionUpdated();
                }

                _pendingMove = false;
                return;
            }

            if (_isDragging)
            {
                _editor.EndMove(worldPosition);
                _isDragging = false;
            }
        }

        public void Undo() => _editor.Undo();
        public void Redo() => _editor.Redo();
    }
}
