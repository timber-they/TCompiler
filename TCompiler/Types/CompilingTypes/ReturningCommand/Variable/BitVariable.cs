namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public abstract class BitVariable : Variable
    {
        protected BitVariable(bool isConstant, bool value, string name = null) : base(isConstant, name)
        {
            Value = value;
        }

        public bool Value { get; }

        public override string ToString() => IsConstant ? $"#{Value}" : Name;
    }
}