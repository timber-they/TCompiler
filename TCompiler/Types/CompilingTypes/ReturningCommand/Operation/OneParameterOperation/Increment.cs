#region

using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation
{
    /// <summary>
    ///     Increases the parameter by 1 and writes the new value into the parameter<br />
    ///     Syntax:<br />
    ///     parameter++
    /// </summary>
    public class Increment : OneParameterOperation
    {
        /// <summary>
        ///     Initiates a new Increment
        /// </summary>
        /// <param name="parameter">The parameter to increase</param>
        public Increment(ByteVariableCall parameter) : base(parameter)
        {
        }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make an increment
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString() => $"inc {((ByteVariableCall) Parameter).ByteVariable}\n" +
                                             $"mov A, {Parameter.Variable.Address}";
    }
}