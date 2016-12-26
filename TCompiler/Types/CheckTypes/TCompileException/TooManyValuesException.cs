namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class TooManyValuesException : TooManyException
    {
        public TooManyValuesException(string message = "There are too many Values! Try making them local.") : base(message)
        {
        }
    }
}