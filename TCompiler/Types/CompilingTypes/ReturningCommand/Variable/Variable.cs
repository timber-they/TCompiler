namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public abstract class Variable : ReturningCommand
    {
        private string _name;
        private byte _value;
        private bool _isValue;

        protected Variable(bool isValue, string name = null, byte value = 0)
        {
            _name = name;
            _isValue = isValue;
            _value = value;
        }
    }
}