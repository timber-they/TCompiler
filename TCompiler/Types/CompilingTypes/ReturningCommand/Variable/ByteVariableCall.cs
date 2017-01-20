namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    ///     The call of a byte variable (e.g. int)<br />
    ///     Syntax:<br />
    ///     4<br />
    ///     or:<br />
    ///     int a<br />
    ///     a
    /// </summary>
    public class ByteVariableCall : VariableCall
    {
        /// <summary>
        ///     Initializes a new ByteVariableCall
        /// </summary>
        /// <param name="byteVariable">The variable that is being called</param>
        public ByteVariableCall(ByteVariable byteVariable) : base(byteVariable)
        {
            ByteVariable = byteVariable;
        }

        /// <summary>
        ///     The variable that is being called
        /// </summary>
        public ByteVariable ByteVariable { get; }

        /// <summary>
        ///     Moves the value of this into the accu
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString()
            => $"mov A, {(!ByteVariable.IsConstant ? ByteVariable.ToString() : $"#{ByteVariable.Value}")}";
    }
}