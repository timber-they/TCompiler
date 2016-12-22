namespace TCompiler.Types.CompilingTypes.Block
{
    public class Break : Command
    {
        private Label _currentBlockEndLabel;

        public Break(Label currentBlockEndLabel)
        {
            _currentBlockEndLabel = currentBlockEndLabel;
        }
    }
}