using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Flux2DEditor.Core.Geometry
{
    public static class Intersection
    {
        private const float EPS = 1e-6f;

        /// <summary>
        /// Result of line segment vs line segment intersection.
        /// </summary>
        public readonly struct SegmentSegmentResult
        {
            public bool Intersects { get; init; }
            public bool Overlap { get; init; }
            public float T { get; init; }
            public float U { get; init; }
            public Vector2 Point { get; init; }
        }

        public static SegmentSegmentResult SegmentSegment(in LineSegment ab, in LineSegment cd)
        {
            var a = ab.A;
            var b = ab.B;
            var c = cd.A;
            var d = cd.B;

            var r = b - a;
            var s = d - c;

            var rxs = Cross(r, s);
            var c_a = c - a;
            var cross_cma_r = Cross(c_a, r);

            // Case 1: Collinear or parallel
            if (MathF.Abs(rxs) < EPS)
            {
                // Collinear (overlapping)
                if (MathF.Abs(cross_cma_r) < EPS)
                {
                    // Use projection of C and D onto AB to determine overlap
                    float rLenSq = r.LengthSquared();
                    if (rLenSq < EPS)
                    {
                        // AB is a point
                        if (Vector2.DistanceSquared(a, c) < EPS)
                        {
                            return new SegmentSegmentResult { Intersects = true, Overlap = true, T = 0, U = 0, Point = a };
                        }
                        return new SegmentSegmentResult { Intersects = false };
                    }

                    float t0 = Vector2.Dot(c - a, r) / rLenSq;
                    float t1 = Vector2.Dot(d - a, r) / rLenSq;

                    float tmin = MathF.Min(t0, t1);
                    float tMax = MathF.Max(t0, t1);

                    // If intervals do not overlap
                    if (tMax < 0 - EPS || tmin > 1 + EPS)
                    {
                        return new SegmentSegmentResult { Intersects = false };
                    }

                    float tOverlap = Math.Clamp(tmin, 0, 1);
                    var p = a + r * tOverlap;

                    return new SegmentSegmentResult
                    {
                        Intersects = true,
                        Overlap = true,
                        T = tOverlap,
                        U = 0,
                        Point = p
                    };
                }

                // Parallel but not collinear
                return new SegmentSegmentResult { Intersects = false };
            }

            // Case 2: Lines intersect at exactly one point
            float t = Cross(c_a, s) / rxs;
            float u = Cross(c_a, r) / rxs;

            if (t >= -EPS && t <= 1 + EPS && u >= -EPS && u <= 1 + EPS)
            {
                var p = a + r * t;
                return new SegmentSegmentResult
                {
                    Intersects = true,
                    Overlap = true,
                    T = t,
                    U = u,
                    Point = p
                };
            }

            return new SegmentSegmentResult { Intersects = false };
        }

        private static float Cross(Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;
    }
}
