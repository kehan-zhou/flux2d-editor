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

        public void BeginMove(Vector2 worldPositon)
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

            _moveContext = new MoveContext(shapeIds, startPosition, grabOffset);

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
                var command = new MoveShapesCommand(_scene, _moveContext.ShapeIds, delta);
                _commandHistory.Execute(command);
            }

            _moveContext = null;
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
