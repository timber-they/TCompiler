#region

using System;
using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    /// Represents the logical or operation<br/>
    /// Syntax:<br/>
    /// paramA | paramB
    /// </summary>
    public class Or : TwoParameterOperation
    {
        /// <summary>
        /// Initializes a new or operation
        /// </summary>
        /// <param name="pars">The parameter for the operation</param>
        public Or(Tuple<VariableCall, VariableCall> pars) : base(pars)
        {
        }

        /// <summary>
        /// Evaluates the stuff to execute in assembler to make a logical or operation
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
            =>
            ParamA is ByteVariableCall
                ? $"mov A, {((ByteVariableCall) ParamA).ByteVariable}\norl A, {((ByteVariableCall) ParamB).ByteVariable}"
                : $"{AssembleCodePreviews.MoveBitToAccu(ParseToAssembler.Label, ParseToAssembler.Label, (BitVariableCall) ParamB)}\n" +
                  $"{AssembleCodePreviews.MoveBitTo(new Bool("C", "c", false), ParseToAssembler.Label, ParseToAssembler.Label, ((BitVariableCall) ParamA).BitVariable)}\n" +
                  $"\norl C, 224.0\nmov 224.0, C";
    }
}