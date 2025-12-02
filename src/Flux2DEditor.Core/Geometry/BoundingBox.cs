using System.Numerics;

namespace Flux2DEditor.Core.Geometry
{
    /// <summary>
    /// Axis-aligned bounding box (AABB), guaranteed to keep Min <= Max.
    /// </summary>
    public readonly struct BoundingBox
    {
        private const float EPS = 1e-5f;

        public Vector2 Min { get; }
        public Vector2 Max { get; }

        public float Width => Max.X - Min.X;
        public float Height => Max.Y - Min.Y;

        public Vector2 Size => Max - Min;
        public Vector2 Center => (Min + Max) * 0.5f;

        public bool IsEmpty => Min.X > Max.X || Min.Y > Max.Y;

        public BoundingBox(Vector2 min, Vector2 max)
        {
            Min = new Vector2(MathF.Min(min.X, max.X), MathF.Min(min.Y, max.Y));
            Max = new Vector2(MathF.Max(min.X, max.X), MathF.Max(min.Y, max.Y));
        }

        public BoundingBox(float x, float y, float width, float height)
        {
            var min = new Vector2(x, y);
            var max = new Vector2(x + width, y + height);

            Min = new Vector2(MathF.Min(min.X, max.X), MathF.Min(min.Y, max.Y));
            Max = new Vector2(MathF.Max(min.X, max.X), MathF.Max(min.Y, max.Y));
        }

        public static readonly BoundingBox Empty = new BoundingBox(
            new Vector2(float.PositiveInfinity, float.PositiveInfinity),
            new Vector2(float.NegativeInfinity, float.NegativeInfinity)
        );

        public bool Contains(Vector2 v)
        {
            if (IsEmpty) return false;

            return v.X >= Min.X - EPS && v.X <= Max.X + EPS &&
                   v.Y >= Min.Y - EPS && v.Y <= Max.Y + EPS;
        }

        public bool Intersects(in BoundingBox other)
        {
            if (IsEmpty || other.IsEmpty) return false;

            return !(other.Max.X < Min.X - EPS ||
                     other.Min.X > Max.X + EPS ||
                     other.Max.Y < Min.Y - EPS ||
                     other.Min.Y > Max.Y + EPS);
        }

        public BoundingBox Translated(Vector2 delta)
        {
            return new BoundingBox(Min + delta, Max + delta);
        }

        public BoundingBox ExpandedToInclude(Vector2 v)
        {
            if (IsEmpty) return new BoundingBox(v, v);

            return new BoundingBox(
                new Vector2(MathF.Min(Min.X, v.X), MathF.Min(Min.Y, v.Y)),
                new Vector2(MathF.Max(Max.X, v.X), MathF.Max(Max.Y, v.Y))
            );
        }

        public BoundingBox Expanded(float amount)
        {
            if (IsEmpty) return this;

            return new BoundingBox(
                new Vector2(Min.X - amount, Min.Y - amount),
                new Vector2(Max.X + amount, Max.Y + amount)
            );
        }

        public BoundingBox Union(in BoundingBox other)
        {
            if (IsEmpty) return other;
            if (other.IsEmpty) return this;

            return new BoundingBox(
                new Vector2(MathF.Min(Min.X, other.Min.X), MathF.Min(Min.Y, other.Min.Y)),
                new Vector2(MathF.Max(Max.X, other.Max.X), MathF.Max(Max.Y, other.Max.Y))
            );
        }
    }
}
