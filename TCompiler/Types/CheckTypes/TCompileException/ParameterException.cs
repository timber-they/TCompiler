namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class ParameterException : TCompileException
    {
        public ParameterException(string message="Wrong parameter!") : base(message)
        {
        }
    }
}