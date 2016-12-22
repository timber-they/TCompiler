namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class Char : Variable
    {
        public Char(bool isConstant, string name = null, byte value = 0) : base(isConstant, name, value)
        {
        }
    }
}