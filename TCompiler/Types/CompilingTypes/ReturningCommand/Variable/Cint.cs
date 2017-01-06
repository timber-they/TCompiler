namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class Cint : ByteVariable
    {
        public Cint(bool isConstant, string address, string name, byte value = 0)
            : base(isConstant, value, address, name)
        {
        }
    }
}