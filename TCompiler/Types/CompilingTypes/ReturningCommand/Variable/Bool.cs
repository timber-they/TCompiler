namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class Bool : BitVariable
    {
        public Bool(bool isConstant, string address, string name, bool value=false) : base(isConstant, value, address, name)
        {
        }
    }
}