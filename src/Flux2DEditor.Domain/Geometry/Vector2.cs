namespace Flux2DEditor.Domain.Geometry
{
    public readonly struct Vector2 : IEquatable<Vector2>
    {
        public double X { get; }
        public double Y { get; }

        public static readonly Vector2 Zero = new(0, 0);

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 Min(Vector2 left, Vector2 right) => new(Math.Min(left.X, right.X), Math.Min(left.Y, right.Y));

        public static Vector2 Max(Vector2 left, Vector2 right) => new(Math.Max(left.X, right.X), Math.Max(left.Y, right.Y));

        public static Vector2 operator +(Vector2 left, Vector2 right) => new(left.X + right.X, left.Y + right.Y);

        public static Vector2 operator -(Vector2 left, Vector2 right) => new(left.X - right.X, left.Y - right.Y);

        public bool Equals(Vector2 other) => X.Equals(other.X) && Y.Equals(other.Y);

        public override bool Equals(object? obj) => obj is Vector2 other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public static bool operator ==(Vector2 left, Vector2 right) => left.Equals(right);

        public static bool operator !=(Vector2 left, Vector2 right) => !left.Equals(right);
    }
}
