#region

using System;

#endregion


namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    ///     A collection of byte variables<br />
    ///     Syntax:<br />
    ///     collection#1 col
    /// </summary>
    public class Collection : Variable //todo xMem implementation
    {
        /// <summary>
        ///     Initializes a new Collection
        /// </summary>
        /// <param name="startAddress">
        ///     The beginning address of the collection. Keep in mind that the collection can't partially be
        ///     in the extendedMemory.
        /// </param>
        /// <param name="name">The name of the collection variable</param>
        /// <param name="rangeCount">The count of items in the collection - must be constant</param>
        public Collection (Address startAddress, string name, int rangeCount) : base (startAddress, name, false) =>
            RangeCount = rangeCount;

        /// <summary>
        ///     The count of items in the collection - always constant
        /// </summary>
        public int RangeCount { get; }

        /// <summary>
        ///     Don't call this. It just throws an exception
        /// </summary>
        /// <param name="variable">Totally uninteresting because it just throws an exception anyway</param>
        /// <returns>Nothing. If you don't count the fact that an exception is thrown...</returns>
        public override string MoveVariableIntoThis (VariableCall variable) => throw new Exception (
                                                                                   "It's quite a strange procedure to move a collection into a collection though");
    }
}