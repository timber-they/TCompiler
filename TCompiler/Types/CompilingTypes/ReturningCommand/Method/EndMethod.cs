namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    public class EndMethod : Command
    {
        public EndMethod(Method method)
        {
            Method = method;
        }

        public Method Method { get; }
    }
}