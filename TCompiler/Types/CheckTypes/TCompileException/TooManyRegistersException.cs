namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class TooManyRegistersException : TooManyException
    {
        public TooManyRegistersException(int line, string message = "There are not enough Registers! You'll have to solve this differently.") : base(line, message)
        {
        }
    }
}