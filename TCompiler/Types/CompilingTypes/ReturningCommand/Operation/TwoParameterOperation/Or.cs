#region

using System;
using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     Represents the logical or operation<br />
    ///     Syntax:<br />
    ///     paramA | paramB
    /// </summary>
    public class Or : TwoParameterOperation
    {
        /// <summary>
        ///     Initializes a new or operation
        /// </summary>
        /// <param name="pars">The parameter for the operation</param>
        public Or(Tuple<ReturningCommand, ReturningCommand> pars) : base(pars)
        {
        }

        public Or(ReturningCommand paramA, ReturningCommand paramB) : base(paramA, paramB)
        { }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a logical or operation
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            var byteVariableCall = ParamB as ByteVariableCall;
            return byteVariableCall != null
                ? $"{ParamA}\nanl A, {byteVariableCall.ByteVariable}"
                : $"{ParamA}\n" +
                  $"{AssembleCodePreviews.MoveBitTo(new Bool(new Address(0x0D0, 7), "c", false), ParseToAssembler.Label, ParseToAssembler.Label, ((BitVariableCall) ParamB).BitVariable)}\n" +
                  "orl C, 224.0\nmov 224.0, C";
        }
    }
}