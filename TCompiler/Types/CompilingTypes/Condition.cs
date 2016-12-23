using TCompiler.Types.CompilingTypes.ReturningCommand.Operation;

namespace TCompiler.Types.CompilingTypes
{
    public class Condition : Command
    {
        private ReturningCommand.ReturningCommand _evaluation;

        public Condition(ReturningCommand.ReturningCommand evaluation)
        {
            _evaluation = evaluation;
        }
    }
}