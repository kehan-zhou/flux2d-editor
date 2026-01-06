namespace Flux2DEditor.Domain.Shapes
{
    public readonly record struct ShapeId
    {
        public Guid Value { get; }

        public ShapeId(Guid value)
        {
            Value = value;
        }

        public static ShapeId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();
    }
}
