using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    public class Assignment : Operation
    {
        protected ReturningCommand Evaluation { get; }
        protected Variable.Variable ToAssign { get; }

        public Assignment(Variable.Variable toAssign, ReturningCommand evaluation)
        {
            ToAssign = toAssign;
            Evaluation = evaluation;
        }

        public override string ToString()
        {
            if (ToAssign is ByteVariable)
            {
                var call = Evaluation as ByteVariableCall;
                return call != null
                    ? $"mov {ToAssign}, {(call.Variable.IsConstant ? "#" + call.Variable.Value : call.Variable.ToString())}"
                    : $"{Evaluation}\nmov {ToAssign}, A";
            }

            if (!(ToAssign is BitOfVariable)) return $"{Evaluation}\nmov C, acc.0\nmov {ToAssign}, C";

            ((BitOfVariable) ToAssign).RegisterLoop = ParseToObjects.CurrentRegister;
            var fin = $"{Evaluation}\n{ToAssign}";
            ParseToObjects.CurrentRegisterAddress--;
            return fin;
        }
    }
}