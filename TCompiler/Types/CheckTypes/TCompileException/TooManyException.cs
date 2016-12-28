namespace TCompiler.Types.CheckTypes.TCompileException
{
    public abstract class TooManyException : TCompileException
    {
        protected TooManyException(int line, string message) : base(line, message)
        {
        }
    }
}