namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class TooManyRegistersException : TCompileException
    {
        public TooManyRegistersException(string message = "There are not enough Registers! You'll have to solve this differently.") : base(message)
        {
        }
    }
}