using System;
using TCompiler.Settings;
using TCompiler.Types.CheckTypes.TCompileException;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    /// A collection of byte variables<br />
    /// Syntax:<br/>
    /// collection#1 col
    /// </summary>
    public class Collection : Variable
    {
        /// <summary>
        /// Initializes a new Collection
        /// </summary>
        /// <param name="startAddress">The beginning address of the collection</param>
        /// <param name="name">The name of the collection variable</param>
        /// <param name="rangeCount">The count of items in the collection - must be constant</param>
        public Collection(Address startAddress, string name, int rangeCount) : base(startAddress, name, false)
        {
            RangeCount = rangeCount;
            var addr = startAddress;
            for (var i = 0; i < rangeCount; i++)
                addr = addr.NextAddress;
            if (addr.IsInExtendedMemory)
                throw new TooManyCollectionsException(GlobalProperties.LineIndex);
        }

        /// <summary>
        /// The count of items in the collection - always constant
        /// </summary>
        public int RangeCount { get; }

        public override string MoveVariableIntoThis(VariableCall variable)
        {
            throw new Exception("It's quite a strange procedure to move a collection into a collection though");
        }
    }
}