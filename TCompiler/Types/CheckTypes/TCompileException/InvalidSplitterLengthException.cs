namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    /// Gets thrown when there's no expected spliter length that equals the splitted line
    /// </summary>
    public class InvalidSplitterLengthException : CompileException
    {
        /// <summary>
        /// Initializes a new InvalidSplitterLengthException
        /// </summary>
        /// <param name="line">The line in which the InvalidSplitterLengthException got thrown</param>
        /// <param name="actualSplitterLength">The actual length of the splitted line</param>
        /// <param name="message">The message to show the user</param>
        public InvalidSplitterLengthException(int line, int actualSplitterLength, string message="The space count ({0}) isn't valid! Try adding/removing an argument.") : base(line, string.Format(message, actualSplitterLength))
        {
        }
    }
}