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
        /// Initializes a new WhileBlock<br/>
        /// Syntax:<br/>
        /// while [boolean expression]
        /// </summary>
        /// <param name="endLabel">The end of the WhileBlock</param>
        /// <param name="condition">The condition that must be true to continue executing the inner part</param>
        /// <param name="upperLabel">The beginning of the while block (The label to jump to to repeat)</param>
        public WhileBlock(Label endLabel, Condition condition, Label upperLabel) : base(endLabel, null)
        {
            Condition = condition;
            UpperLabel = upperLabel;
        }

        /// <summary>
        /// The condition that must be true to continue executing the inner part
        /// </summary>
        public Condition Condition { get; }
        /// <summary>
        /// The beginning of the while block (The label to jump to to repeat)
        /// </summary>
        public Label UpperLabel { get; }
    }
}