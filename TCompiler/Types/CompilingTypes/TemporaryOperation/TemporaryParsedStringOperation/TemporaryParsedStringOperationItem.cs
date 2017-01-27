namespace TCompiler.Types.CompilingTypes.TemporaryOperation.TemporaryParsedStringOperation
{
    public class TemporaryParsedStringOperationItem
    {
        public TemporaryParsedStringOperationItem(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString() => Value;
    }
}