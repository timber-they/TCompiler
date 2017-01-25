namespace TCompiler.Types.CompilingTypes.Block
{
    /// <summary>
    ///     Kind of the same as a normal if block in C#<br />
    ///     Syntax:<br />
    ///     if [booleanExpression]
    /// </summary>
    public class IfBlock : Block
    {
        /// <summary>
        ///     Initializes a new IfBlock
        /// </summary>
        /// <param name="endLabel">The end of the IfBlock</param>
        /// <param name="condition">The condition to execute the inner part</param>
        /// <param name="else">
        ///     The ElseBlock included in the IfBlock
        ///     <remarks>Doesn't have to be included, can be null as well</remarks>
        /// </param>
        public IfBlock(Label endLabel, Condition condition, ElseBlock @else) : base(endLabel)
        {
            Condition = condition;
            Else = @else;
        }

        /// <summary>
        ///     The condition to execute the inner part
        /// </summary>
        public Condition Condition { get; }

        /// <summary>
        ///     The ElseBlock included in the IfBlock
        ///     <remarks>
        ///         Doesn't have to be included, can be null as well
        ///     </remarks>
        /// </summary>
        public ElseBlock Else { get; set; }
    }
}