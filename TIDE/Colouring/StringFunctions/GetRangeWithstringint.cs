using System.Collections.Generic;
using TIDE.Colouring.Types;

namespace TIDE.Colouring.StringFunctions
{
    public static class GetRangeWithStringInt
    {
        public static Intint GetRangeWithStringIntSpaces(Stringint that, IReadOnlyList<string> strings)
        {
            var start = 0;
            for (var i = 0; i < that.Theint; i++)
                start += strings[i].Length + 1;
            return new Intint(start, start + strings[that.Theint].Length);
        }
    }
}