namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class InvalidNameException : CompileException
    {
        public InvalidNameException(int line, string message = "The name entered is not valid!") : base(line, message)
        {
        }
    }
}