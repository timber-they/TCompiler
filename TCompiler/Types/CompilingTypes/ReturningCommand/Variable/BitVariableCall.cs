using TCompiler.Types.CompilerTypes;


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
        /// <param name="cLine">The original T code line</param>
        public BitVariableCall (BitVariable bitVariable, CodeLine cLine) : base (bitVariable, cLine) =>
            BitVariable = bitVariable;

        /// <summary>
        ///     The bitVariable that is being called
        /// </summary>
        public BitVariable BitVariable { get; }

        /// <summary>
        ///     Moves the bit of this into the accu
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString ()
            =>
                BitVariable.MoveThisIntoAcc0 ();
    }
}