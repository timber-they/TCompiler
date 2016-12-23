using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class Assignment : Operation
    {
        private Variable.Variable _toAssign;
        private ReturningCommand _evalutation;

        public Assignment(Variable.Variable toAssign, ReturningCommand evalutation)
        {
            _toAssign = toAssign;
            _evalutation = evalutation;
        }

        public override string ToString()
            =>
            _toAssign is ByteVariable
                ? $"{_evalutation}\nmov {_toAssign}, A"
                : $"{_evalutation}\nmov C, acc.0\nmov {_toAssign}, C";
    }
}