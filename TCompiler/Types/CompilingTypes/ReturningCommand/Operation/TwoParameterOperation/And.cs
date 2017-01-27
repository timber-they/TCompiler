#region

using System;
using TCompiler.AssembleHelp;
using TCompiler.Compiling;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     Evaluates the logical result of paramA and paramB<br />
    ///     Syntax:<br />
    ///     paramA & paramB
    /// </summary>
    public class And : TwoParameterOperation
    {
        /// <summary>
        ///     Initiates a new and operation
        /// </summary>
        /// <param name="pars">The parameter for the operation</param>
        public And(Tuple<ReturningCommand, ReturningCommand> pars) : base(pars)
        {
        }

        public And(ReturningCommand paramA, ReturningCommand paramB) : base (paramA, paramB)
        { }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a logical and
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
        {
            var byteVariableCall = ParamB as ByteVariableCall;
            return byteVariableCall != null
                ? $"{ParamA}\nanl A, {byteVariableCall.ByteVariable}"
                : $"{ParamA}\n" +
                  $"{AssembleCodePreviews.MoveBitTo(new Bool(new Address(0x0D0, 7), "c", false), ParseToAssembler.Label, ParseToAssembler.Label, ((BitVariableCall) ParamB).BitVariable)}\n" +
                  "anl C, 224.0\nmov 224.0, C";
        }
    }
}