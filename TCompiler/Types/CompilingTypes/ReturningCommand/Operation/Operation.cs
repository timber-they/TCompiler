using System.Collections.Generic;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation
{
    public abstract class Operation : ReturningCommand
    {
        protected Operation(bool deactivateEa, bool activateEa, IEnumerable<int> expectedSplitterLength ) : base(deactivateEa, activateEa, expectedSplitterLength)
        {
        }
    }
}