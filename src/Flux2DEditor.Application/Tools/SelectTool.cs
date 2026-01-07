using Flux2DEditor.Application.Selection;
using Flux2DEditor.Domain.Geometry;
using Flux2DEditor.Domain.Scene;
using Flux2DEditor.Domain.Shapes;

namespace Flux2DEditor.Application.Tools
{
    public sealed class SelectTool : ITool
    {
        private readonly ISelectionService _selection;

        private Vector2? _boxStart;
        private Vector2? _boxCurrent;

        public bool IsBoxSelecting => _boxStart != null;

        public BoundingBox? CurrentBox
        {
            get
            {
                if (_boxStart == null || _boxCurrent == null) return null;

                var min = Vector2.Min(_boxStart.Value, _boxCurrent.Value);
                var max = Vector2.Max(_boxStart.Value, _boxCurrent.Value);
                return new BoundingBox(min, max);
            }
        }

        public SelectTool(ISelectionService selection)
        {
            _selection = selection;
        }

        public void OnActivate()
        {
            // Intentionally empty.
            // Selection is persistent across tool activation.
        }

        public void OnDeactivate()
        {
            // Intentionally empty.
            // Selection lifecycle is not owned by the tool.
        }

        public void SelectSingle(HitTestResult hit)
        {
            _selection.Clear();

            if (hit.HitShapeId is ShapeId shapeId)
            {
                _selection.Select(shapeId);
            }
        }

        public void BeginBoxSelect(Vector2 start)
        {
            _boxStart = start;
            _boxCurrent = start;
        }

        public void UpdateBoxSelect(Vector2 current)
        {
            if (_boxStart == null) return;
            _boxCurrent = current;
        }

        public void EndBoxSelect(Scene scene)
        {
            if (_boxStart == null || _boxCurrent == null) return;

            var min = Vector2.Min(_boxStart.Value, _boxCurrent.Value);
            var max = Vector2.Max(_boxStart.Value, _boxCurrent.Value);
            var box = new BoundingBox(min, max);

            _selection.Clear();

            foreach (var shape in scene.Shapes)
            {
                if (box.Intersects(shape.GetBoundingBox()))
                {
                    _selection.Select(shape.Id);
                }
            }

            _boxStart = null;
            _boxCurrent = null;
        }
    }
}
