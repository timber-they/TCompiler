namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public abstract class BitVariable : Variable
    {
        protected BitVariable(bool isConstant, bool value, string address, string name) : base(isConstant, address, name)
        {
            Value = value;
        }

        public bool Value { get; }

        public override string ToString() => IsConstant ? $"#{Value}" : base.ToString();
    }
}