#region

using TCompiler.AssembleHelp;
using TCompiler.Settings;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    ///     The call of a bit variable (of a bool)<br />
    ///     Syntax:<br />
    ///     true<br />
    ///     or:<br />
    ///     bool b := true<br />
    ///     b
    /// </summary>
    public class BitVariableCall : VariableCall
    {
        /// <summary>
        ///     Initializes a new BitVariableCall
        /// </summary>
        /// <param name="bitVariable">The bitVariable that is being called</param>
        public BitVariableCall(BitVariable bitVariable) : base(bitVariable)
        {
            BitVariable = bitVariable;
        }

        /// <summary>
        ///     The bitVariable that is being called
        /// </summary>
        public BitVariable BitVariable { get; }

        /// <summary>
        ///     Moves the bit of this into the accu
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
            =>
            AssembleCodePreviews.MoveBitToAccu(GlobalProperties.Label, GlobalProperties.Label, this);
    }
}