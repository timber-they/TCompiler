namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class ByteVariableCall : VariableCall
    {
        public ByteVariableCall(ByteVariable byteVariable) : base(byteVariable)
        {
            ByteVariable = byteVariable;
        }

        public ByteVariable ByteVariable { get; }

        public override string ToString()
            => $"mov A, {(!ByteVariable.IsConstant ? ByteVariable.ToString() : $"#{ByteVariable.Value}")}";
    }
}