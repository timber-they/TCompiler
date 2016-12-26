namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class InvalidCommandException : TCompileException
    {
        public string Command { get; }

        public InvalidCommandException(string command, string message="The entered Command isn't valid!") : base(message)
        {
            Command = command;
        }
    }
}