#region

using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     Divides paramB from paramA<br />
    ///     Syntax:<br />
    ///     paramA / paramB
    /// </summary>
    public class Divide : TwoParameterOperation
    {
        /// <summary>
        ///     Initiates a new divide operation
        /// </summary>
        /// <param name="paramA">The first parameter to divide from</param>
        /// <param name="paramB">The second parameter that divides the first one</param>
        public Divide(ReturningCommand paramA, ReturningCommand paramB) : base(paramA, paramB)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a divide operation
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString() => $"{ParamA}\nmov 0F0h, {((ByteVariableCall) ParamB).ByteVariable}\ndiv AB";
    }
}