using TCompiler.Compiling;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class VariableCall : ReturningCommand
    {
        private Variable _variable;

        public VariableCall(Variable variable)
        {
            _variable = variable;
        }

        public override string ToString()
            =>
            _variable is ByteVariable
                ? $"mov A, {(!_variable.IsConstant ? _variable.Name : $"#{((ByteVariable) _variable).Value}")}"
                : AssembleHelp.AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label1, (BitVariable) _variable);
    }
}