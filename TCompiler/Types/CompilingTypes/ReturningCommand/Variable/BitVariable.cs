namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class BitVariable : Variable
    {
        private bool _value;

        public BitVariable(bool isConstant, bool value, string name = null) : base(isConstant, name)
        {
            _value = value;
        }
    }
}