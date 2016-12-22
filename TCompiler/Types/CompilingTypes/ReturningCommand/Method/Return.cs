namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    public class Return : ReturningCommand
    {
        private ReturningCommand _toReturn;

        public Return(ReturningCommand toReturn)
        {
            _toReturn = toReturn;
        }
    }
}