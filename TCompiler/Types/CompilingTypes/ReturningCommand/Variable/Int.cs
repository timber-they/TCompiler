namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class Int : ByteVariable
    {
        public Int(bool isConstant, string name = null, byte value = 0) : base(isConstant, value, name)
        {
        }
    }
}