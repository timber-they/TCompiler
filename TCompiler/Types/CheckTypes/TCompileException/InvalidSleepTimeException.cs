namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class InvalidSleepTimeException : TCompileException
    {
        public InvalidSleepTimeException(int line, int value, string message = "This won't worl with that time") : base(line, message)
        {
            Value = value;
        }

        public int Value { get; }
    }
}