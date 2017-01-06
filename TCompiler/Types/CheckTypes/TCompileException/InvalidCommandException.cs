namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class InvalidCommandException : CompileException
    {
        public InvalidCommandException(int line, string command, string message = "The entered Command isn't valid!")
            : base(line, message)
        {
            Command = command;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private string Command { get; }
    }
}