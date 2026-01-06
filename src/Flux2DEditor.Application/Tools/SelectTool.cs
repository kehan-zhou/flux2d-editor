using Flux2DEditor.Application.Selection;
using Flux2DEditor.Domain.Shapes;


namespace Flux2DEditor.Application.Tools
{
    public sealed class SelectTool : ITool
    {
        private readonly ISelectionService _selection;

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

        public void Select(HitTestResult hit)
        {
            if (hit.HitShapeId is ShapeId shapeId)
            {
                _selection.Clear();
                _selection.Select(shapeId);
            }
            else
            {
                _selection.Clear();
            }
        }
    }
}
