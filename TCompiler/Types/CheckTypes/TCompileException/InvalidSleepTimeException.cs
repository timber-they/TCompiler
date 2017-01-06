namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class InvalidSleepTimeException : CompileException
    {
        public InvalidSleepTimeException(int line, int value, string message = "This won't work with that time")
            : base(line, message)
        {
            Value = value;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private int Value { get; }
    }
}