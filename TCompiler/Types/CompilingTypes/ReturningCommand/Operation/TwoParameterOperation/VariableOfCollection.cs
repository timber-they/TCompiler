using System.Collections.Generic;
using System.Text;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class VariableOfCollection : Operation
    {
        public VariableOfCollection(Collection collection, ByteVariableCall collectionIndex) : base(true, true, new List<int> {1,2,3})
        {
            Collection = collection;
            CollectionIndex = collectionIndex;
        }

        private Collection Collection { get; }
        private ByteVariableCall CollectionIndex { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(CollectionIndex.ToString());
            sb.AppendLine($"add A, #{Collection.Address}");
            sb.AppendLine("mov R0, A");
            sb.AppendLine("mov A, @R0");
            return sb.ToString();
        }
    }
}