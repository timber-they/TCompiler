#region

using System;

#endregion


namespace TCompiler.Types.CompilingTypes.TemporaryOperation.TemporarOperationRepresentation
{
    /// <summary>
    ///     Represents an operation sign
    /// </summary>
    public class TemporarOperationSignRepresentation : TemporarOperationItemRepresentation
    {
        /// <summary>
        ///     Initializes a new temporar operation sign representation
        /// </summary>
        /// <param name="value">The value of the operation sign (e.g. "+" )</param>
        /// <param name="leftRightParameterRequired">Indicates wether the right/left parameter for this operation is neccessary</param>
        public TemporarOperationSignRepresentation (string value, Tuple <bool, bool> leftRightParameterRequired)
            : base (value) => LeftRightParameterRequired = leftRightParameterRequired;

        /// <summary>
        ///     Indicates wether the right/left parameter for this operation is neccessary
        /// </summary>
        public Tuple <bool, bool> LeftRightParameterRequired { get; }
    }
}