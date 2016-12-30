namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    public class Return : ReturningCommand
    {
        private readonly ReturningCommand _toReturn;

        public Return(ReturningCommand toReturn)
        {
            _toReturn = toReturn;
        }


        public override string ToString() => $"{_toReturn}\nret";
    }
}