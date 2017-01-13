using System.Collections.Generic;

namespace TCompiler.Types.CompilingTypes
{
    /// <summary>
    /// The base for every command you can type in T
    /// </summary>
    public abstract class Command
    {
        protected Command(bool deactivateEa, bool activateEa, IEnumerable<int> expectedSplitterLengths)
        {
            DeactivateEa = deactivateEa;
            ActivateEa = activateEa;
            ExpectedSplitterLengths = expectedSplitterLengths;
        }

        public bool DeactivateEa { get; }
        public bool ActivateEa { get; }
        public IEnumerable<int> ExpectedSplitterLengths { get; protected set; }
    }
}