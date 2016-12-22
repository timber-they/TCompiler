namespace TCompiler.Types.CompilingTypes
{
    public class Condition : Command
    {
        private Command _evaluation;

        public Condition(Command evaluation)
        {
            _evaluation = evaluation;
        }
    }
}