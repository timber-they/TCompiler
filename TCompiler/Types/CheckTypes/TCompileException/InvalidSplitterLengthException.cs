namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class InvalidSplitterLengthException : CompileException
    {
        public InvalidSplitterLengthException(int line, int spaceCount, string message="The space count ({0}) isn't valid! Try adding/removing an argument.") : base(line, string.Format(message, spaceCount))
        {
        }
    }
}