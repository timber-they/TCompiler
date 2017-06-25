#region

using System;
using System.Collections.Generic;

using TCompiler.Enums;
using TCompiler.Types.CompilerTypes;

#endregion


namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    /// <summary>
    ///     The Interrupt Service Routine (ISR) method<br />
    ///     Syntax:<br />
    ///     ISRExternal0<br />
    ///     or<br />
    ///     ISRExternal1
    /// </summary>
    public class InterruptServiceRoutine : Method
    {
        /// <summary>
        ///     Initiates a new ISR
        /// </summary>
        /// <param name="label">The label to call to call the ISR</param>
        /// <param name="interruptType">The type of the interrupt this ISR listens to</param>
        /// <param name="startValue">
        ///     If existent the start value for the timer/counter (256 - startValue = start value) as a byte
        ///     tuple (high/low)
        /// </param>
        /// <param name="cLine">The original T code line</param>
        public InterruptServiceRoutine (
            Label label, InterruptType interruptType, Tuple<byte, byte> startValue,
            CodeLine cLine)
            : base (null, new List<Variable.Variable> (), label, cLine)
        {
            InterruptType = interruptType;
            StartValue = startValue;
        }

        /// <summary>
        ///     The type of the interrupt this ISR listens to
        /// </summary>
        public InterruptType InterruptType { get; }

        /// <summary>
        ///     If existent the start value for the timer/counter (256 - startValue = start value) as a byte tuple (high/low)
        /// </summary>
        public Tuple<byte, byte> StartValue { get; }
    }
}