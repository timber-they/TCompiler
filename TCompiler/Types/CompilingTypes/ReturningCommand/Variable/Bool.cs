namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class Bool : BitVariable
    {
        public Bool(bool isConstant, string name = null, bool value = false) : base(isConstant, value, name)
        {
        }
    }
}