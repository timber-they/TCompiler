using System.Text;
using TCompiler.AssembleHelp;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    /// The collection variable used to assign a variable from a collection
    /// </summary>
    internal class VariableOfCollectionVariable : ByteVariable
    {
        /// <summary>
        /// Initializes a new collection
        /// </summary>
        /// <param name="collection">The collection from which the variable is assigned</param>
        /// <param name="collectionIndex">The index of the variable in the collection</param>
        public VariableOfCollectionVariable(Collection collection, ByteVariableCall collectionIndex)
            : base(false, 0, collection.Address, $"{collection.Address}:{collectionIndex.ByteVariable}")
        {
            Collection = collection;
            CollectionIndex = collectionIndex;
        }

        /// <summary>
        /// The collection from which the variable is assigned
        /// </summary>
        private Collection Collection { get; }
        /// <summary>
        /// The index of the variable in the collection
        /// </summary>
        private ByteVariableCall CollectionIndex { get; }

        /// <summary>
        /// Moves the accu into the variable
        /// </summary>
        /// <returns></returns>
        public override string MoveAccuIntoThis()
        {
            var sb = new StringBuilder();
            sb.AppendLine(AssembleCodePreviews.MoveAccuIntoB());
            sb.AppendLine(CollectionIndex.ToString());
            sb.AppendLine($"add A, #{Collection.Address}");
            sb.AppendLine("mov R0, A");
            sb.AppendLine("mov @R0, 0F0h");
            return sb.ToString();
        }

        /// <summary>
        /// Moves the B-Register in this variable of the collection
        /// </summary>
        /// <returns>The assembler code to execute as a string</returns>
        public override string MoveBIntoThis() => $"mov A, B\n{MoveAccuIntoThis()}";

        /// <summary>
        /// Moves another variable into this variable of the collection
        /// </summary>
        /// <param name="variable">The other variable to take the value from</param>
        /// <returns>The assembler code to execute as a string</returns>
        public override string MoveVariableIntoThis(VariableCall variable)
            => $"{variable}\n{MoveAccuIntoThis()}";
    }
}
