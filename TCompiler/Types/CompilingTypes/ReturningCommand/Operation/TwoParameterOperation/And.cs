#region

using System;
using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    /// Evaluates the logical result of paramA and paramB<br/>
    /// Syntax:<br/>
    /// paramA & paramB
    /// </summary>
    public class And : TwoParameterOperation
    {
        /// <summary>
        /// Initiates a new and operation
        /// </summary>
        /// <param name="pars">The parameter for the operation</param>
        public And(Tuple<VariableCall, VariableCall> pars) : base(pars)
        {
        }

        /// <summary>
        /// Evaluates the stuff to execute in assembler to make a logical and
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString() => ParamA is ByteVariableCall && ParamB is ByteVariableCall
            ? $"mov A, {((ByteVariableCall) ParamA).ByteVariable}\nanl A, {((ByteVariableCall) ParamB).ByteVariable}"
            : $"{AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label, ParseToAssembler.Label, (BitVariableCall) ParamB)}\n" +
              $"{AssembleCodePreviews.MoveBitTo(new Bool(false, "C", "c"), ParseToAssembler.Label, ParseToAssembler.Label, ((BitVariableCall) ParamA).BitVariable)}\n" +
              "anl C, acc.0\nmov acc.0, C";
    }
}