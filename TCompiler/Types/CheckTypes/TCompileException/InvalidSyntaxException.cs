namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class InvalidSyntaxException : TCompileException
    {
        public InvalidSyntaxException(string message="The syntax isn't correct.") : base(message)
        {
        }
    }
}