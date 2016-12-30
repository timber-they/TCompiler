using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class Assignment : Operation
    {
        private readonly ReturningCommand _evalutation;
        private readonly Variable.Variable _toAssign;

        public Assignment(Variable.Variable toAssign, ReturningCommand evalutation)
        {
            _toAssign = toAssign;
            _evalutation = evalutation;
        }

        public override string ToString()
            => _toAssign is ByteVariable
                ? (_evalutation is ByteVariableCall
                    ? $"mov {_toAssign}, {(((ByteVariableCall) _evalutation).Variable.IsConstant ? "#" + ((ByteVariableCall) _evalutation).Variable.Value : ((ByteVariableCall) _evalutation).Variable.Name)}"
                    : $"{_evalutation}\nmov {_toAssign}, A")
                : $"{_evalutation}\nmov C, acc.0\nmov {_toAssign}, C";
    }
}