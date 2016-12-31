namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class NormalErrorException : CompileException
    {
        public NormalErrorException(Error.Error error) : base(error.Line, error.Message)
        {
        }
    }
}