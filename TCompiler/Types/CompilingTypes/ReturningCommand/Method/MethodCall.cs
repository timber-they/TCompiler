namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    public class MethodCall : ReturningCommand
    {
        private Method _method;

        public MethodCall(Method method)
        {
            _method = method;
        }

        public override string ToString() => $"call {_method.Name}";
    }
}