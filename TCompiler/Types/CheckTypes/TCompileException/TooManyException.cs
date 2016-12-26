namespace TCompiler.Types.CheckTypes.TCompileException
{
    public abstract class TooManyException : TCompileException
    {
        protected TooManyException(string message) : base(message)
        {
        }
    }
}