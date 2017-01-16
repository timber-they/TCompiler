using System.Collections.Generic;
using TCompiler.Enums;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

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
        /// <param name="count">If existent the count for the timer/counter (256 - count = start value)</param>
        public InterruptServiceRoutine(Label label, InterruptType interruptType, ByteVariableCall count) : base(null, new List<Variable.Variable>(), label)
        {
            InterruptType = interruptType;
            Count = count;
        }

        /// <summary>
        /// The type of the interrupt this ISR listens to
        /// </summary>
        public InterruptType InterruptType { get; }

        public ByteVariableCall Count { get; }
    }
}