using System.Collections.Generic;
using TIDE.Types;

namespace TIDE.StringFunctions
{
    public static class GetRangeWithStringInt
    {
        public static intint GetRangeWithStringIntSpaces(stringint that, IReadOnlyList<string> strings)
        {
            var start = 0;
            for (var i = 0; i < that.Theint; i++)
                start += strings[i].Length + 1;
            return new intint(start, start+strings[that.Theint].Length);
        }
    }
}