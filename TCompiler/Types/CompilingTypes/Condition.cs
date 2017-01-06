namespace TCompiler.Types.CompilingTypes
{
    public class Condition : Command
    {
        public Condition(ReturningCommand.ReturningCommand evaluation)
        {
            Evaluation = evaluation;
        }

        private ReturningCommand.ReturningCommand Evaluation { get; }

        public override string ToString() => Evaluation.ToString();
    }
}