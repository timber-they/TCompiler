namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public abstract class ByteVariable : Variable
    {
        protected ByteVariable(bool isConstant, byte value, string name = null) : base(isConstant, name)
        {
            Value = value;
        }

        public byte Value { get; }

        public override string ToString() => IsConstant ? $"#{Value}" : Name;
    }
}