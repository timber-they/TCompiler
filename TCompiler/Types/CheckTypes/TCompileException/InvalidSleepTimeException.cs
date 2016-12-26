namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class InvalidSleepTimeException : TCompileException
    {
        public InvalidSleepTimeException(int value, string message = "This won't worl with that time") : base(message)
        {
            Value = value;
        }

        public int Value { get; }
    }
}