using Flux2DEditor.Domain.Shapes;

namespace Flux2DEditor.Application.Selection
{
    public interface ISelectionService
    {
        IReadOnlyCollection<ShapeId> SelectedShapeIds { get; }

        bool IsSelected(ShapeId shapeId);

        void Select(ShapeId shapeId);

        void Deselect(ShapeId shapeId);

        void Clear();

        void ReplaceWith(IEnumerable<ShapeId> shapeIds);
    }
}
