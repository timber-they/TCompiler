#region

using System.Collections.Generic;

#endregion

namespace TCompiler.Types.CompilingTypes
{
    /// <summary>
    ///     The base for every command you can type in T
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        ///     Initiates a new Command
        /// </summary>
        /// <param name="deactivateEa">A boolean that indicates wether the enableAll flag must get deactivated before the command</param>
        /// <param name="activateEa">A boolean that indicates wether the enableAll flag must get activated after the command</param>
        /// <param name="expectedSplitterLengths">The length that is expected for the splitter length of the line of the command</param>
        protected Command(bool deactivateEa, bool activateEa, IEnumerable<int> expectedSplitterLengths)
        {
            DeactivateEa = deactivateEa;
            ActivateEa = activateEa;
            ExpectedSplitterLengths = expectedSplitterLengths;
        }

        /// <summary>
        ///     A boolean that indicates wether the enableAll falg must get deactivated before the command
        /// </summary>
        public bool DeactivateEa { get; }

        /// <summary>
        ///     A boolean that indicates wether the enableAll falg must get activated after the command
        /// </summary>
        public bool ActivateEa { get; }

        /// <summary>
        ///     The length that is expected for the splitter length of the line of the command
        /// </summary>
        public IEnumerable<int> ExpectedSplitterLengths { get; }
    }
}