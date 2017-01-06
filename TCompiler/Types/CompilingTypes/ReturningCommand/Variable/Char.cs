namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class Char : ByteVariable
    {
        public Char(bool isConstant, string address, string name, byte value = 0)
            : base(isConstant, value, address, name)
        {
        }
    }
}