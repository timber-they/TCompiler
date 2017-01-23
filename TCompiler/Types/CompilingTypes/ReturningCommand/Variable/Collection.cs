using System.Collections.Generic;
using System.Linq;
using TCompiler.Compiling;
using TCompiler.Types.CheckTypes.TCompileException;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class Collection : Variable

    {
        public Collection(Address startAddress, string name, bool isConstant, int rangeCount) : base(startAddress, name, isConstant)
        {
            RangeCount = rangeCount;
        }

        public int RangeCount { get; }

        private List<Address> AddresRange()
        {
            var fin = new List<Address> {Address};
            for (var i = 1; i < RangeCount; i++)
                fin.Add(fin.Last().NextAddress);
            return fin;
        }

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