namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class InvalidValueException : TCompileException
    {
        public InvalidValueException(int line, string message = "The value entered is not valid for this type")
            : base(line, message)
        {
        }
    }
}