namespace TCompiler.Types.CompilingTypes
{
    public class Condition
    {
        private Command _evaluation;

        public Condition(Command evaluation)
        {
            _evaluation = evaluation;
        }
    }
}