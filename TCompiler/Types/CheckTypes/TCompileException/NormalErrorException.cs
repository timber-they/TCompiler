namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class NormalErrorException : TCompileException
    {
        public NormalErrorException(Error.Error error) : base(error.Message)
        {
        }
    }
}