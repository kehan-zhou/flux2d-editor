using Flux2DEditor.Application.Commands;
using Flux2DEditor.Application.Selection;
using Flux2DEditor.Application.Tools;
using Flux2DEditor.Domain.Geometry;
using Flux2DEditor.Domain.Handles;
using Flux2DEditor.Domain.Scene;
using Flux2DEditor.Domain.Shapes;
using System.Transactions;

namespace Flux2DEditor.Application.Editor
{
    public sealed class EditorController
    {
        private readonly Scene _scene;
        private readonly ISelectionService _selectionService;
        private readonly CommandHistory _commandHistory;

        private ITool? _activeTool;
        private MoveContext? _moveContext;
        private ResizeContext? _resizeContext;

        public event Action? RequestRedraw;

        public MoveContext? CurrentMove => _moveContext;
        public ResizeContext? CurrentResize => _resizeContext;

        public ITool? ActiveTool => _activeTool;

        public IReadOnlyCollection<ShapeId> SelectedShapeIds => _selectionService.SelectedShapeIds;

        public EditorController(Scene scene, ISelectionService selectionService, CommandHistory commandHistory)
        {
            _scene = scene;
            _selectionService = selectionService;
            _commandHistory = commandHistory;
        }

        private void TriggerRedraw()
        {
            RequestRedraw?.Invoke();
        }

        public void CancelInteraction()
        {
            _moveContext = null;

            if (_activeTool is ICancellableTool cancellable)
            {
                cancellable.Cancel();
            }

            TriggerRedraw();
        }

        public void NotifyInteractionUpdated()
        {
            TriggerRedraw();
        }

        public void SetActiveTool(ITool tool)
        {
            _activeTool?.OnDeactivate();
            _activeTool = tool;
            _activeTool.OnActivate();
        }

        public void BeginMove(Vector2 worldPositon, bool isCopy)
        {
            if (_selectionService.SelectedShapeIds.Count == 0)
            {
                return;
            }

            var shapeIds = _selectionService.SelectedShapeIds.ToList();
            var primaryId = shapeIds[0];
            var primaryShape = _scene.Get(primaryId);

            var startPosition = primaryShape.GetBoundingBox().Min;
            var grabOffset = worldPositon - startPosition;

            _moveContext = new MoveContext(shapeIds, startPosition, grabOffset, isCopy);

            if (isCopy)
            {
                var copies = shapeIds.Select(id => _scene.Get(id).Clone()).ToList();

                _moveContext.SetPreviewCopies(copies);
            }

            TriggerRedraw();
        }

        public void UpdateMove(Vector2 worldPositon, bool axisLock)
        {
            if (_moveContext == null)
            {
                return;
            }

            _moveContext.Update(worldPositon, axisLock);

            TriggerRedraw();
        }

        public void EndMove(Vector2 worldPositon)
        {
            if (_moveContext == null)
            {
                return;
            }

            Vector2 delta = _moveContext.CurrentDelta;

            if (delta != Vector2.Zero)
            {
                if (_moveContext.IsCopy && _moveContext.PreviewCopies != null)
                {
                    var movedCopies = _moveContext.PreviewCopies.Select(s => s.Translate(delta)).ToList();
                    var copyCommand = new CopyShapesCommand(_scene, movedCopies);
                    _commandHistory.Execute(copyCommand);

                    _selectionService.Clear();
                    _selectionService.ReplaceWith(copyCommand.Copies.Select(s => s.Id));
                }
                else
                {
                    _commandHistory.Execute(new MoveShapesCommand(_scene, _moveContext.ShapeIds, delta));
                }
            }

            _moveContext = null;
            TriggerRedraw();
        }

        public void BeginResize(ShapeId shapeId, HandleType handle)
        {
            var shape = _scene.Get(shapeId);
            _resizeContext = new ResizeContext(shapeId, handle, shape);
            TriggerRedraw();
        }

        public void UpdateResize(Vector2 worldPositon)
        {
            if (_resizeContext == null) return;

            _resizeContext.Update(worldPositon);
            TriggerRedraw();
        }

        public void EndResize()
        {
            if (_resizeContext == null) return;

            var finalShape = _resizeContext.PreviewShape;
            if (finalShape != null)
            {
                var before = _resizeContext.OriginalShape;
                var after = finalShape;
                _commandHistory.Execute(new ResizeShapeCommand(_scene, before, after));
            }

            _resizeContext = null;
            TriggerRedraw();
        }

        public void Undo()
        {
            _commandHistory.Undo();

            TriggerRedraw();
        }

        public void Redo()
        {
            _commandHistory.Redo();

            TriggerRedraw();
        }
    }
}
