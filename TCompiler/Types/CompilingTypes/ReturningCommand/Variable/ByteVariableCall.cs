namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class ByteVariableCall : VariableCall
    {
        public ByteVariableCall(ByteVariable variable)
        {
            Variable = variable;
        }

        public ByteVariable Variable { get; }

        public override string ToString()
            => $"mov A, {(!Variable.IsConstant ? Variable.Name : $"#{Variable.Value}")}";
    }
}