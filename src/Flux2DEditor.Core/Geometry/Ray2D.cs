using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Flux2DEditor.Core.Geometry
{
    /// <summary>
    /// Represents a 2D ray with an origin (O) and a normalized direction (D).
    /// The ray extends infinitely in direction D.
    /// </summary>
    public readonly struct Ray2D
    {
        private const float EPS = 1e-6f;

        public Vector2 Origin { get; }
        public Vector2 Direction { get; }

        public Ray2D(Vector2 origin, Vector2 direction)
        {
            Origin = origin;

            if (direction.LengthSquared() < EPS)
            {
                Direction = new Vector2(1, 0);
            }
            else
            {
                Direction = Vector2.Normalize(direction);
            }
        }

        /// <summary>
        /// Gets a point on the ray: O + t * D, where t >= 0.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector2 GetPoint(float t) => Origin + Direction * t;

        /// <summary>
        /// Returns distance from point to the infinite ray.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public float DistanceToPoint(Vector2 p)
        {
            var op = p - Origin;
            var t = Vector2.Dot(op, Direction);
            if (t < 0) // point is "behind" the ray origin
            {
                return Vector2.Distance(p, Origin);
            }
            var proj = Origin + Direction * t;
            return Vector2.Distance(p, proj);
        }

        /// <summary>
        /// Checks if a point lies on the ray (within epsilon).
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool ContainsPoint(Vector2 p)
        {
            var op = p - Origin;
            var cross = Cross(op, Direction);

            // must lie almost on the line
            if (MathF.Abs(cross) > EPS)
            {
                return false;
            }

            return Vector2.Dot(op, Direction) >= -EPS;
        }

        /// <summary>
        /// Intersects ray with a line segment.
        /// </summary>
        /// <param name="seg"></param>
        /// <returns>(hit, distance, hitPoint)</returns>
        public (bool hit, float t, Vector2 point) IntersectSegment(in LineSegment seg)
        {
            var a = seg.A;
            var b = seg.B;

            var v1 = Origin - a;
            var v2 = b - a;
            var v3 = new Vector2(-Direction.Y, Direction.X); // perpendicular (90°)

            float dot = Vector2.Dot(v2, v3);
            if (MathF.Abs(dot) < EPS)
            {
                return (false, 0, Vector2.Zero); // parallel
            }

            float t1 = Cross(v2, v1) / dot;
            float t2 = Vector2.Dot(v1, v3) / dot;

            // t1: ray parameter (>=0)
            // t2: segment parameter [0,1]
            if (t1 >= 0 && t2 >= -EPS && t2 <= 1 + EPS)
            {
                var p = GetPoint(t1);
                return (true, t1, p);
            }

            return (false, 0, Vector2.Zero);
        }

        public (bool hit, float tmin) IntersectBoundingBox(in BoundingBox box)
        {
            if (box.IsEmpty)
            {
                return (false, 0);
            }

            float tmin = float.NegativeInfinity;
            float tmax = float.PositiveInfinity;

            // X axis
            if (MathF.Abs(Direction.X) < EPS)
            {
                if (Origin.X < box.Min.X || Origin.X > box.Max.X)
                {
                    return (false, 0);
                }
            }
            else
            {
                float tx1 = (box.Min.X - Origin.X) / Direction.X;
                float tx2 = (box.Max.X - Origin.X) / Direction.X;
                tmin = MathF.Max(tmin, MathF.Min(tx1, tx2));
                tmax = MathF.Min(tmax, MathF.Max(tx2, tx1));
            }

            // Y axis
            if (MathF.Abs(Direction.Y) < EPS)
            {
                if (Origin.Y < box.Min.Y || Origin.Y > box.Max.Y)
                {
                    return (false, 0);
                }
            }
            else
            {
                float ty1 = (box.Min.Y - Origin.Y) / Direction.Y;
                float ty2 = (box.Max.Y - Origin.Y) / Direction.Y;
                tmin = MathF.Max(tmin, MathF.Min(ty1, ty2));
                tmax = MathF.Min(tmax, MathF.Max(ty1, ty2));
            }

            if (tmax >= tmin && tmax > 0)
            {
                float hitT = tmin > 0 ? tmin : tmax;
                return (true, hitT);
            }

            return (false, 0);
        }

        private static float Cross(Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;

        public override string ToString() => $"Ray2D(O={Origin}, D={Direction})";
    }
}
