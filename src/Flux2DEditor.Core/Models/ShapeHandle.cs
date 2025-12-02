using Flux2DEditor.Core.Geometry;

namespace Flux2DEditor.Core.Models
{
    /// <summary>
    /// Represents a single control handle for a shape.
    /// </summary>
    public record ShapeHandle(BoundingBox Box, int Index);
}