namespace TCompiler.Types.CompilingTypes.TemporaryOperation.TemporarOperationRepresentation
{
    /// <summary>
    ///     Represents an operation item
    /// </summary>
    public class TemporarOperationItemRepresentation
    {
        /// <summary>
        ///     Initializes a new temporar operation item representation
        /// </summary>
        /// <param name="value">The value of the operation typed in T</param>
        protected TemporarOperationItemRepresentation(string value)
        {
            Value = value;
        }

        /// <summary>
        ///     The value of the operation typed in T
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     The value
        /// </summary>
        /// <returns>The value</returns>
        public override string ToString() => Value;
    }
}