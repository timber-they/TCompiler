namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class Int : ByteVariable
    {
        public Int(bool isConstant, string address, string name, byte value = 0)
            : base(isConstant, value, address, name)
        {
        }
    }
}