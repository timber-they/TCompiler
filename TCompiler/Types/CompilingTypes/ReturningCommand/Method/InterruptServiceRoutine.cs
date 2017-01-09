using System.Collections.Generic;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    public class InterruptServiceRoutine : Method
    {
        public InterruptServiceRoutine(Label label, bool externalInterruptServiceRoutine0, bool externalInterruptServiceRoutine1) : base(null, new List<Variable.Variable>(), label)
        {
            ExternalInterruptServiceRoutine0 = externalInterruptServiceRoutine0;
            ExternalInterruptServiceRoutine1 = externalInterruptServiceRoutine1;
        }

        public bool ExternalInterruptServiceRoutine0 { get; }
        public bool ExternalInterruptServiceRoutine1 { get; }
    }
}