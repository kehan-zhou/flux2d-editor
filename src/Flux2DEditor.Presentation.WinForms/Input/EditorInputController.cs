using Flux2DEditor.Application.Editor;
using Flux2DEditor.Application.HitTesting;
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

        public EditorInputController(EditorController editor, HitTestService hitTestService, Scene scene)
        {
            _editor = editor;
            _hitTestService = hitTestService;
            _scene = scene;
        }

        public void OnPointerDown(Vector2 worldPosition)
        {
            var hit = _hitTestService.HitTest(_scene, worldPosition);
            _editor.HandleSelection(hit);

            if (hit.HitShapeId != null)
            {
                _editor.BeginMove(worldPosition);
                _isDragging = true;
            }
        }

        public void OnPointerMove(Vector2 worldPosition)
        {
            if (_isDragging)
            {
                _editor.UpdateMove(worldPosition);
            }
        }

        public void OnPointerUp(Vector2 worldPosition)
        {
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
