using System.Numerics;

namespace Flux2DEditor.Core.Geometry
{
    /// <summary>
    /// Represents a 2D line segment defined by two end points A and B (closed interval [A, B]).
    /// Geometry-only; no rendering or platform dependencies.
    /// </summary>
    public readonly struct LineSegment
    {
        private const float EPS = 1e-5f;

        public Vector2 A { get; }
        public Vector2 B { get; }

        public Vector2 Direction => B - A;
        public float Length => Direction.Length();
        public float LengthSquared => Direction.LengthSquared();

        public LineSegment(Vector2 a, Vector2 b)
        {
            A = a;
            B = b;
        }

        /// <summary>
        /// Returns a bounding box that fully contains the line segment.
        /// </summary>
        /// <returns></returns>
        public BoundingBox GetBoundingBox()
        {
            return new BoundingBox(A, B);
        }

        /// <summary>
        /// Projects a point onto the infinite line AB and returns parameter t and projected point
        /// t = 0 -> A, t = 1 -> B. If the segment is a point, t = 0.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public (float t, Vector2 point) ProjectPoint(Vector2 p)
        {
            var ab = Direction;
            var abLenSq = ab.LengthSquared();

            if (abLenSq < EPS)
            {
                return (0f, A);
            }

            var ap = p - A;
            var t = Vector2.Dot(ab, ap) / abLenSq;
            var clamped = Math.Clamp(t, 0f, 1f);

            return (clamped, A + ab * clamped);
        }

        /// <summary>
        /// Returns the shortest distance from a point to this line segment.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public float DistanceToPoint(Vector2 p)
        {
            var (_, proj) = ProjectPoint(p);
            return Vector2.Distance(p, proj);
        }

        /// <summary>
        /// Returns true if a point lies on the segment within epsilon tolerance.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool ContainsPoint(Vector2 p)
        {
            var (t, proj) = ProjectPoint(p);
            return t >= -EPS && t <= 1 + EPS && Vector2.DistanceSquared(p, proj) < EPS * EPS;
        }

        /// <summary>
        /// Gets a point at parameter t in [0, 1].
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector2 Lerp(float t)
        {
            return A + (B - A) * t;
        }

        /// <summary>
        /// Returns true if the segment length is extremely small.
        /// </summary>
        public bool IsDegenerate => LengthSquared < EPS * EPS;

        public override string ToString() => $"LineSegment(A={A}, B={B})";
    }
}
