#region

using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     Evaluates paramA modulo paramB<br />
    ///     Syntax:<br />
    ///     paramA % paramB
    /// </summary>
    public class Modulo : TwoParameterOperation
    {
        /// <summary>
        ///     Initiates a new modulo operation
        /// </summary>
        /// <param name="paramA">The first modulo parameter</param>
        /// <param name="paramB">The second modulo parameter</param>
        public Modulo(ByteVariableCall paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a modulo operation
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
            => $"{ParamA}\nmov B, {((ByteVariableCall) ParamB).ByteVariable}\ndiv AB\nxch A, B";
    }
}