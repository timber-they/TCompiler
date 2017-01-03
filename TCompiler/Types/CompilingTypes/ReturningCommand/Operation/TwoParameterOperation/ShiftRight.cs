using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class ShiftRight : TwoParameterOperation
    {
        private readonly Label _label;
        private readonly string _register;

        public ShiftRight(VariableCall paramA, VariableCall paramB, string register, Label label) : base(paramA, paramB)
        {
            _register = register;
            _label = label;
        }

        public override string ToString()
            => $"clr C\n{ParamB}\nmov {_register}, A\n{ParamA}\n{_label}:\nrrc A\naddc A, #0\ndjnz {_register}, {_label}";
    }
}