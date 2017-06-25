#region

using System.Text;
using TCompiler.AssembleHelp;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    ///     The collection variable used to assign a variable from a collection
    /// </summary>
    internal class VariableOfCollectionVariable : ByteVariable
    {
        /// <summary>
        ///     Initializes a new collection
        /// </summary>
        /// <param name="collection">The collection from which the variable is assigned</param>
        /// <param name="collectionIndex">The index of the variable in the collection</param>
        public VariableOfCollectionVariable(Collection collection, ByteVariableCall collectionIndex)
            : base(false, 0, collection.Address, $"{collection.Address}:{collectionIndex.ByteVariable.Address}")
        {
            Collection = collection;
            CollectionIndex = collectionIndex;
        }

        /// <summary>
        ///     The collection from which the variable is assigned
        /// </summary>
        private Collection Collection { get; }

        /// <summary>
        ///     The index of the variable in the collection
        /// </summary>
        private ByteVariableCall CollectionIndex { get; }

        /// <summary>
        ///     Moves the accu into the variable
        /// </summary>
        /// <returns></returns>
        public override string MoveAccuIntoThis()
        {
            var sb = new StringBuilder();
            sb.AppendLine(AssembleCodePreviews.MoveAccuIntoB());
            sb.AppendLine(CollectionIndex.ToString());

            if (!Collection.Address.IsInExtendedMemory)
            {
                sb.AppendLine($"{Ac.Add} A, #{Collection.Address}");
                sb.AppendLine($"{Ac.Move} R0, A");
                sb.AppendLine($"{Ac.Move} @R0, 0F0h");
            }
            else
            {
                sb.AppendLine(Collection.Address.MoveThisIntoDataPointer());
                sb.AppendLine($"{Ac.Add} A, 082h");
                sb.AppendLine($"{Ac.Move} 082h, A");
                sb.AppendLine($"{Ac.Move} A, 083h");
                sb.AppendLine($"{Ac.Add}c A, #0");
                sb.AppendLine($"{Ac.Move} 083h, A");
                sb.AppendLine($"{Ac.Move} A, 0F0h");
                sb.AppendLine($"{Ac.MoveExtended} @dptr, A");
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Moves the B-Register in this variable of the collection
        /// </summary>
        /// <returns>The assembler code to execute as a string</returns>
        public override string MoveBIntoThis() => $"{Ac.Move} A, B\n{MoveAccuIntoThis()}";

        /// <summary>
        ///     Moves another variable into this variable of the collection
        /// </summary>
        /// <param name="variable">The other variable to take the value from</param>
        /// <returns>The assembler code to execute as a string</returns>
        public override string MoveVariableIntoThis(VariableCall variable)
            => $"{variable}\n{MoveAccuIntoThis()}";
    }
}