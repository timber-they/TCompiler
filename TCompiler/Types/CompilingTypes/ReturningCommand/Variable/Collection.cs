using System.Collections.Generic;
using System.Linq;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public class Collection : Variable

    {
        public Collection(Address startAddress, string name, bool isConstant, int rangeCount) : base(startAddress, name, isConstant)
        {
            RangeCount = rangeCount;
        }

        private int RangeCount { get; }

        public List<Address> AddresRange()
        {
            var fin = new List<Address> {Address};
            for (var i = 1; i < RangeCount; i++)
                fin.Add(fin.Last().NextAddress);
            return fin;
        }
    }
}