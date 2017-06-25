namespace TCompiler.Types.CompilingTypes.TemporaryOperation.TemporarOperationRepresentation
{
    /// <summary>
    ///     Represents a bracket. Nothing more than that
    /// </summary>
    public class TemporarBracketRepresentation : TemporarOperationItemRepresentation
    {
        /// <summary>
        ///     Initializes a new temporar bracket representation
        /// </summary>
        /// <param name="value">The value of the bracket ( "(" / ")" </param>
        protected TemporarBracketRepresentation (string value) : base (value) {}
    }
}