namespace TCompiler.Types.CompilingTypes.Block
{
    /// <summary>
    /// Kindof similar to the C# WhileBlock - repeats it content while the condition is true<br/>
    /// Syntax:<br/>
    /// while [booleanExpression]
    /// </summary>
    public class WhileBlock : Block
    {
        /// <summary>
        /// Initializes a new WhileBlock
        /// </summary>
        /// <param name="endLabel">The end of the WhileBlock</param>
        /// <param name="condition">The condition that must be true to continue executing the inner part</param>
        /// <param name="upperLabel"></param>
        public WhileBlock(Label endLabel, Condition condition, Label upperLabel) : base(endLabel)
        {
            Condition = condition;
            UpperLabel = upperLabel;
        }

        public Condition Condition { get; }
        public Label UpperLabel { get; }
    }
}