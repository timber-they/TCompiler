using TCompiler.Types.CompilerTypes;

namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class FileDoesntExistException : CompileException
    {
        public FileDoesntExistException(CodeLine codeLine, string fileName,
            string message = "The file {0} doesn't exist!") : base(codeLine, string.Format(message, fileName))
        {
        }
    }
}