using System.Collections.Generic;
using System.Linq;
using TCompiler.Compiling;
using TCompiler.Types.CheckTypes.TCompileException;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    /// A collection of byte variables<br>
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
        }

        /// <summary>
        /// The count of items in the collection - always constant
        /// </summary>
        public int RangeCount { get; }

        /// <summary>
        /// Evaluates the range of addresses the collection covers
        /// </summary>
        /// <returns>The range as a list of the addresses</returns>
        private List<Address> AddresRange()
        {
            var fin = new List<Address> {Address};
            for (var i = 1; i < RangeCount; i++)
                fin.Add(fin.Last().NextAddress);
            return fin;
        }

        /// <summary>
        /// Evaluates the address at a specific collection index
        /// </summary>
        /// <param name="index">The collection index</param>
        /// <returns>The address as an Address</returns>
        public Address GetAddress(int index)
        {
            if (index < 0)
                throw new InvalidValueException(ParseToAssembler.Line, index.ToString(),
                    "The index {0} must be positive.");
            var range = AddresRange();
            if (range.Count <= index)
                throw new InvalidValueException(ParseToAssembler.Line, index.ToString(),
                    "The index {0} was too big for this collection.");
            return range[index];
        }
    }
}