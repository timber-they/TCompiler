#region

using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    public class Assignment : Operation
    {
        public Assignment(Variable.Variable toAssign, ReturningCommand evaluation)
        {
            ToAssign = toAssign;
            Evaluation = evaluation;
        }

        protected ReturningCommand Evaluation { get; }
        protected Variable.Variable ToAssign { get; }

        public override string ToString()
        {
            if (ToAssign is ByteVariable)
            {
                var call = Evaluation as ByteVariableCall;
                return call != null
                    ? $"mov {ToAssign}, {(call.Variable.IsConstant ? "#" + call.Variable.Value : call.Variable.ToString())}"
                    : $"{Evaluation}\nmov {ToAssign}, A";
            }

            var count = 0;
            var bitOfVariable = ToAssign as BitOfVariable;
            var bitOf = Evaluation as BitOf;

            if (bitOfVariable != null)
            {
                bitOfVariable.RegisterLoop = ParseToObjects.CurrentRegister;
                count++;
            }
            if (bitOf != null)
            {
                bitOf.RegisterLoop = ParseToObjects.CurrentRegister;
                count++;
            }

            var fin = $"{Evaluation}\n{((BitVariable)ToAssign).MoveAcc0IntoThis()}";
            ParseToObjects.CurrentRegisterAddress-= count;
            return fin;
        }
    }
}