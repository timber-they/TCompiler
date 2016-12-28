namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class ParameterException : TCompileException
    {
        public ParameterException(int line, string message ="Wrong parameter!") : base(line, message)
        {
        }
    }
}