namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class Cint : ByteVariable
    {
        public Cint(bool isConstant, string name = null, byte value = 0) : base(isConstant, value, name)
        {
        }
    }
}