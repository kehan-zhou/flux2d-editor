namespace Flux2DEditor.Domain.Geometry
{
    public static class GeometryUtil
    {
        public static double DistancePointToSegment(Vector2 point, Vector2 a, Vector2 b)
        {
            var ab = b - a;
            var ap = point - a;

            var abLengthSquared = ab.LengthSquared();
            if (abLengthSquared == 0)
            {
                return (point - a).Length();
            }
            var t = Vector2.Dot(ap, ab) / abLengthSquared;
            t = Math.Clamp(t, 0.0, 1.0);

            var closest = a + ab * t;
            return (point - closest).Length();
        }
    }
}
