namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class Char : ByteVariable
    {
        public Char(bool isConstant, string name = null, byte value = 0) : base(isConstant, value, name)
        {
        }
    }
}