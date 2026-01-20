using Flux2DEditor.Domain.Geometry;

namespace Flux2DEditor.Domain.Shapes
{
    public sealed class LineSegment : Shape
    {
        public Vector2 Start { get; }
        public Vector2 End { get; }

        public LineSegment(ShapeId id, Vector2 start, Vector2 end) : base(id)
        {
            Start = start;
            End = end;
        }

        public override BoundingBox GetBoundingBox()
        {
            var min = Vector2.Min(Start, End);
            var max = Vector2.Max(Start, End);
            return new BoundingBox(min, max);
        }

        public override bool HitTest(Vector2 point, double tolerance) 
            => GeometryUtil.DistancePointToSegment(point, Start, End) <= tolerance;

        public override Shape Translate(Vector2 delta)
            => new LineSegment(Id, Start + delta, End + delta);

        public override Shape Clone()
            => new LineSegment(ShapeId.New(), Start, End);
        
    }
}
