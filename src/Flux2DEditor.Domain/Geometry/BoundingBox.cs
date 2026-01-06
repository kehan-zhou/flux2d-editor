using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flux2DEditor.Domain.Geometry
{
    public readonly struct BoundingBox
    {
        public Vector2 Min { get; }
        public Vector2 Max { get; }

        public BoundingBox(Vector2 min, Vector2 max)
        {
            if (min.X > max.X || min.Y > max.Y)
            {
                throw new ArgumentException("Min must be less than or equal to Max.");
            }

            Min = min;
            Max = max;
        }

        public double Width => Max.X - Min.X;
        public double Height => Max.Y - Min.Y;

        public bool Contains(Vector2 point)
        {
            return point.X >= Min.X && point.X <= Max.X
                && point.Y >= Min.Y && point.Y <= Max.Y;
        }

        public bool Intersects(BoundingBox other)
        {
            return !(other.Max.X < Min.X
                  || other.Min.X > Max.X
                  || other.Max.Y < Min.Y
                  || other.Min.Y > Max.Y);
        }

        public static BoundingBox FromPositionAndSize(Vector2 position, Vector2 size)
        {
            return new BoundingBox(position, position + size);
        }

        public BoundingBox Translate(Vector2 delta)
        {
            return new BoundingBox(Min + delta, Max + delta);
        }
    }
}
