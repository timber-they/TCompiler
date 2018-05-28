#region

using System.Text;

using TCompiler.AssembleHelp;
using TCompiler.Types.CompilerTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion


namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     A variable of a defined collection. Used to access the value of a variable of a collection.<br />
    ///     Syntax:<br />
    ///     int i := col:1
    /// </summary>
    public class VariableOfCollection : Operation
    {
        /// <summary>
        ///     Initializes a new variable of collection used to get a single value from a collection
        /// </summary>
        /// <param name="collection">The collection from which the item is taken</param>
        /// <param name="collectionIndex">The index of the item in the collection</param>
        /// <param name="cLine">The original T code line</param>
        public VariableOfCollection (
            Collection collection, ReturningCommand collectionIndex,
            CodeLine   cLine) : base (true, true, cLine)
        {
            Collection      = collection;
            CollectionIndex = collectionIndex;
        }

        /// <summary>
        ///     The collection from which the item is taken
        /// </summary>
        private Collection Collection { get; }

        /// <summary>
        ///     The index of the item in the collection
        /// </summary>
        private ReturningCommand CollectionIndex { get; }

        /// <summary>
        ///     Evaluates the assembler code to execute to move the value of the variable into the Accumulator
        /// </summary>
        /// <returns>The assembler code to execute as a string</returns>
        public override string ToString ()
        {
            var sb = new StringBuilder ();
            sb.AppendLine (CollectionIndex.ToString ());
            if (!Collection.Address.IsInExtendedMemory)
            {
                sb.AppendLine ($"{Ac.Add} A, #{Collection.Address}");
                sb.AppendLine ($"{Ac.Move} R0, A");
                sb.AppendLine ($"{Ac.Move} A, @R0");
            }
            else
            {
                sb.AppendLine (Collection.Address.MoveThisIntoDataPointer ());
                sb.AppendLine ($"{Ac.Add} A, 082h");
                sb.AppendLine ($"{Ac.Move} 082h, A");
                sb.AppendLine ($"{Ac.Move} A, 083h");
                sb.AppendLine ($"{Ac.Add}c A, #0");
                sb.AppendLine ($"{Ac.Move} 083h, A");
                sb.AppendLine ($"{Ac.MoveExtended} A, @dptr");
            }

            return sb.ToString ();
        }
    }
}