namespace TCompiler.Types.CompilingTypes.Block
{
    public class Break : Command
    {
        public Label CurrentBlockEndLabel { get; }

        public Break(Label currentBlockEndLabel)
        {
            CurrentBlockEndLabel = currentBlockEndLabel;
        }
    }
}