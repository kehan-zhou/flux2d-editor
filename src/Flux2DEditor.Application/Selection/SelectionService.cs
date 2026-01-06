using Flux2DEditor.Domain.Shapes;

namespace Flux2DEditor.Application.Selection
{
    public sealed class SelectionService : ISelectionService
    {
        private readonly HashSet<ShapeId> _selected = [];

        public IReadOnlyCollection<ShapeId> SelectedShapeIds => _selected;

        public bool IsSelected(ShapeId shapeId) => _selected.Contains(shapeId);

        public void Select(ShapeId shapeId)
        {
            _selected.Add(shapeId);
        }

        public void Deselect(ShapeId shapeId)
        {
            _selected.Remove(shapeId);
        }

        public void Clear()
        {
            _selected.Clear();
        }
    }
}
