namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class BitVariable : Variable
    {
        public bool Value { get; }

        public BitVariable(bool isConstant, bool value, string name = null) : base(isConstant, name)
        {
            Value = value;
        }

        public override string ToString() => IsConstant ? $"#{Value}" : Name;
    }
}