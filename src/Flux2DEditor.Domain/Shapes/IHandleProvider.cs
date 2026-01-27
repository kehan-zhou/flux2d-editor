using Flux2DEditor.Domain.Handles;

namespace Flux2DEditor.Domain.Shapes
{
    public interface IHandleProvider
    {
        IEnumerable<Handle> GetHandles();
    }
}
