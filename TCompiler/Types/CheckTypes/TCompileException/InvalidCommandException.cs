namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class InvalidCommandException : TCompileException
    {
        public string Command { get; }

        public InvalidCommandException(int line, string command, string message="The entered Command isn't valid!") : base(line, message)
        {
            Command = command;
        }
    }
}