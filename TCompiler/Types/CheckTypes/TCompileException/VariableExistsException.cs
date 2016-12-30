namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class VariableExistsException : TCompileException
    {
        public VariableExistsException(int line, string message = "The entered variable name already exists!")
            : base(line, message)
        {
        }
    }
}