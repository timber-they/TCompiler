using System.Collections.Generic;

namespace TCompiler.Types.CompilingTypes.ReturningCommand
{
    /// <summary>
    /// The base class for every command that returns something.
    /// </summary>
    /// <remarks>In the end the result will be saved in the Accu or in the first bit (acc.0) of the Accu</remarks>
    public abstract class ReturningCommand : Command
    {
        /// <summary>
        /// Every returning command can get converted to assembler directly with the toString method
        /// </summary>
        /// <remarks>Every inheriting class has to implement this method</remarks>
        /// <returns>The assembler code for the returning command</returns>
        public abstract override string ToString();

        /// <summary>
        /// Initializes a new ReturningCommand
        /// </summary>
        /// <param name="deactivateEa">A boolean that indicates wether the enableAll flag must get deactivated before the command</param>
        /// <param name="activateEa">A boolean that indicates wether the enableAll flag must get activated after the command</param>
        /// <param name="expectedSplitterLengths">The length that is expected for the splitter length of the line of the command</param>
        protected ReturningCommand(bool deactivateEa, bool activateEa, IEnumerable<int> expectedSplitterLengths ) : base(deactivateEa, activateEa, expectedSplitterLengths)
        {
        }
    }
}