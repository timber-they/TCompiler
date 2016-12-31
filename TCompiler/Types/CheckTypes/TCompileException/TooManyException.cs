namespace TCompiler.Types.CheckTypes.TCompileException
{
    public abstract class TooManyException : CompileException
    {
        protected TooManyException(int line, string message) : base(line, message)
        {
        }
    }
}