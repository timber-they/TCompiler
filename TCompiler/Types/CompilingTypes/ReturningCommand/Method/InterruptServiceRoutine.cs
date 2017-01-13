using System.Collections.Generic;
using TCompiler.Enums;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    /// <summary>
    /// The Interrupt Service Routine (ISR) method<br/>
    /// Syntax:<br/>
    /// ISRExternal0<br/>
    /// or<br/>
    /// ISRExternal1
    /// </summary>
    public class InterruptServiceRoutine : Method
    {
        /// <summary>
        /// Initiates a new ISR
        /// </summary>
        /// <param name="label">The label to call to call the ISR</param>
        /// <param name="interruptType">The type of the interrupt this ISR listens to</param>
        public InterruptServiceRoutine(Label label, InterruptType interruptType) : base(null, new List<Variable.Variable>(), label)
        {
            InterruptType = interruptType;
        }

        /// <summary>
        /// The type of the interrupt this ISR listens to
        /// </summary>
        public InterruptType InterruptType { get; }
    }
}