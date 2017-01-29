using System;

namespace TCompiler.Types.CompilingTypes.TemporaryOperation.TemporaryParsedStringOperation
{
    public class OperationSign : TemporaryParsedStringOperationItem
    {
        public OperationSign(string value, Tuple<bool, bool> leftRightParameterRequired) : base(value)
        {
            LeftRightParameterRequired = leftRightParameterRequired;
        }

        public Tuple<bool, bool> LeftRightParameterRequired { get; }
    }
}