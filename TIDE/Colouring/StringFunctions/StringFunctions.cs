using System.Collections.Generic;
using System.Linq;

namespace TIDE.Colouring.StringFunctions
{
    public static class StringFunctions
    {
        public static List<char> GetRemoved(string before, string after)
        {
            var b = before.ToCharArray().ToList();
            var a = after.ToCharArray().ToList();

            foreach (var c in a)
            {
                if (!b.Contains(c))
                    return new List<char>();
                b.Remove(c);
            }
            return b;
        }
    }
}