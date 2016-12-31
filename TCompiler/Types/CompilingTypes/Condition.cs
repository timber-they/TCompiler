namespace TCompiler.Types.CompilingTypes
{
    public class Condition : Command
    {
        private ReturningCommand.ReturningCommand Evaluation { get; }

        public Condition(ReturningCommand.ReturningCommand evaluation)
        {
            Evaluation = evaluation;
        }

        public override string ToString() => Evaluation.ToString();
    }
}