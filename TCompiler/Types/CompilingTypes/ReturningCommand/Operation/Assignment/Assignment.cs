﻿#region

using System.Linq;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    public class Assignment : Operation
    {
        public Assignment(Variable.Variable toAssign, ReturningCommand evaluation) : base(true, true,
                evaluation.ExpectedSplitterLengths?.Concat(evaluation.ExpectedSplitterLengths?.Select(i => i + 1))
                    .Concat(evaluation.ExpectedSplitterLengths?.Select(i => i + 2))
                    .Distinct())
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
                    ? $"mov {ToAssign}, {(call.ByteVariable.IsConstant ? "#" + call.ByteVariable.Value : call.ByteVariable.ToString())}"
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