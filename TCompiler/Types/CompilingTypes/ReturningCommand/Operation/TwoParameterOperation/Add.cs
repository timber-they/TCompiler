#region

using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     Adds the two parameters<br />
    ///     Syntax:<br />
    ///     paramA + paramB
    /// </summary>
    public class Add : TwoParameterOperation
    {
        /// <summary>
        ///     Initiates a new Add operation
        /// </summary>
        /// <param name="paramA">The first parameter to add</param>
        /// <param name="paramB">The second parameter to add</param>
        public Add(ReturningCommand paramA, ByteVariableCall paramB) : base(paramA, paramB)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make an add operation
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString() => $"{ParamA}\nadd A, {((ByteVariableCall) ParamB).ByteVariable}";
    }
}