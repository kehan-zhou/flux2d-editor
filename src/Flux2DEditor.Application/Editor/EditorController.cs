using Flux2DEditor.Application.Commands;
using Flux2DEditor.Application.Selection;
using Flux2DEditor.Application.Tools;
using Flux2DEditor.Domain.Geometry;
using Flux2DEditor.Domain.Scene;
using Flux2DEditor.Domain.Shapes;

namespace Flux2DEditor.Application.Editor
{
    public sealed class EditorController
    {
        private readonly Scene _scene;
        private readonly ISelectionService _selectionService;
        private readonly CommandHistory _commandHistory;

        private ITool? _activeTool;
        private MoveContext? _moveContext;

        public event Action? RequestRedraw;

        public MoveContext? CurrentMove => _moveContext;

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

        private static Shape MoveShape(Shape shape, Vector2 delta)
        {
            return shape switch
            {
                Rectangle rect => rect.WithPosition(rect.Position + delta),
                _ => throw new NotSupportedException($"Move not supported for shape type {shape.GetType().Name}"),
            };
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

        public void UpdateMove(Vector2 worldPositon)
        {
            if (_moveContext == null)
            {
                return;
            }

            _moveContext.Update(worldPositon);

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
                    var movedCopies = _moveContext.PreviewCopies.Select(s => MoveShape(s, delta)).ToList();
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
