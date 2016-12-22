namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public abstract class ByteVariable : Variable
    {
        private byte _value;

        protected ByteVariable(bool isConstant, byte value, string name = null) : base(isConstant, name)
        {
            _value = value;
        }
    }
}