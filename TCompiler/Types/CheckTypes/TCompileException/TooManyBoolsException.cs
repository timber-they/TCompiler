namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class TooManyBoolsException : TooManyException
    {
        public TooManyBoolsException(int line, string message ="There are too many Boolean values! Try making them local.") : base(line, message)
        {
        }
    }
}