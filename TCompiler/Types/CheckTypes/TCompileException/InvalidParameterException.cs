using TCompiler.Types.CompilerTypes;


namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class InvalidParameterException : CompileException
    {
        /// <inheritdoc />
        public InvalidParameterException (string index,
            CodeLine codeLine, string message = "The {0} parameter is not valid for this operation.") : base (
            codeLine, string.Format(message, index)) {}
    }
}