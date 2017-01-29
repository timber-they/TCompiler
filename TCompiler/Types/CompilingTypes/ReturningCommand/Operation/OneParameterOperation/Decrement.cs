#region

using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation
{
    /// <summary>
    ///     Decreases the parameter by 1 and writes the new value into the parameter<br />
    ///     Syntax:<br />
    ///     parameter--
    /// </summary>
    public class Decrement : OneParameterOperation
    {
        /// <summary>
        ///     Initializes a new decrement
        /// </summary>
        /// <param name="parameter">The variable to decrease</param>
        public Decrement(ReturningCommand parameter) : base(parameter)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a decrement
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString() => $"dec {((ByteVariableCall) Parameter).ByteVariable}\n" +
                                             ((ByteVariableCall)Parameter).ByteVariable.MoveThisIntoAccu();
    }
}