namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class InternalException : CompileException
    {
        public InternalException(int line, string exceptionMessage, string message="An internal error ({0}) occurred. I feel really sorry about that.") : base(line, string.Format(message, exceptionMessage))
        {
        }
    }
}