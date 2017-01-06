#region

using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class Declaration : Command
    {
        public Declaration(Assignment assignment)
        {
            Assignment = assignment;
        }

        private Assignment Assignment { get; }

        public override string ToString() => Assignment?.ToString() ?? "";
    }
}