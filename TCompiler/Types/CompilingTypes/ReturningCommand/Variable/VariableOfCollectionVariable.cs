using System.Text;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    class VariableOfCollectionVariable : Variable
    {
        public VariableOfCollectionVariable(Collection collection, ByteVariableCall collectionIndex) : base(collection.Address, $"{collection.Address}:{collectionIndex.ByteVariable}", false)
        {
            Collection = collection;
            CollectionIndex = collectionIndex;
        }

        private Collection Collection { get; }
        private ByteVariableCall CollectionIndex { get; }

        public string MoveAccuIntoThis()
        {
            var sb = new StringBuilder();
            sb.AppendLine("mov 0F0h, A");
            sb.AppendLine(CollectionIndex.ToString());
            sb.AppendLine($"add A, #{Collection.Address}");
            sb.AppendLine("mov R0, A");
            sb.AppendLine("mov @R0, 0F0h");
            return sb.ToString();
        }
    }
}
