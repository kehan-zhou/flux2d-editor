using System.Numerics;

namespace Flux2DEditor.Core.Geometry
{
    public static class BoundingBoxExtensions
    {
        public static BoundingBox FromPoints(IEnumerable<Vector2> pts)
        {
            var bb = BoundingBox.Empty;
            foreach (var p in pts)
            {
                bb = bb.ExpandedToInclude(p);
            }
            return bb;
        }
    }
}
