namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public abstract class ByteVariable : Variable
    {
        public byte Value { get; }

        protected ByteVariable(bool isConstant, byte value, string name = null) : base(isConstant, name)
        {
            Value = value;
        }
    }
}