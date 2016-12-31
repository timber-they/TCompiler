using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public class Assignment : Operation
    {
        private ReturningCommand Evaluation { get; }
        private Variable.Variable ToAssign { get; }

        public Assignment(Variable.Variable toAssign, ReturningCommand evaluation)
        {
            ToAssign = toAssign;
            Evaluation = evaluation;
        }

        public override string ToString()
            => ToAssign is ByteVariable
                ? (Evaluation is ByteVariableCall
                    ? $"mov {ToAssign}, {(((ByteVariableCall) Evaluation).Variable.IsConstant ? "#" + ((ByteVariableCall) Evaluation).Variable.Value : ((ByteVariableCall) Evaluation).Variable.Name)}"
                    : $"{Evaluation}\nmov {ToAssign}, A")
                : $"{Evaluation}\nmov C, acc.0\nmov {ToAssign}, C";
    }
}