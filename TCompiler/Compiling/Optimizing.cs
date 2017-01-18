using System;
using System.Collections.Generic;

namespace TCompiler.Compiling
{
    public static class Optimizing
    {
        public static string GetOptimizedAssemblerCode(string oldCode)
        {
            throw new NotImplementedException();
        }

        private static List<string> GetCountLast(IReadOnlyList<string> items, int count, int position)
        {
            var fin = new List<string>();
            for (var i = position - 1; i >= 0 && position - i <= count; i--)
                fin.Add(items[i]);
            return fin;
        }
    }
}