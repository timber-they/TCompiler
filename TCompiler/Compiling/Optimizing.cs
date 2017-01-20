#region

using System;
using System.Collections.Generic;

#endregion

namespace TCompiler.Compiling
{
    /// <summary>
    ///     A class providing methods for optimizing the code
    /// </summary>
    public static class Optimizing
    {
        /// <summary>
        ///     Evaluates the optimized assembler code
        /// </summary>
        /// <param name="oldCode">The code that shall get optimized</param>
        /// <returns>The optimized code as a string</returns>
        public static string GetOptimizedAssemblerCode(string oldCode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets the last bunch of items in the list, while the amount of items is defined with count.
        /// </summary>
        /// <param name="items">The items from where to take the items from</param>
        /// <param name="count">The count of items</param>
        /// <param name="position">The starting position in the list, from where the count is being reversal counted</param>
        /// <returns>A list of the last few items</returns>
        // ReSharper disable once UnusedMember.Local
        private static List<string> GetCountLast(IReadOnlyList<string> items, int count, int position)
        {
            var fin = new List<string>();
            for (var i = position - 1; (i >= 0) && (position - i <= count); i--)
                fin.Add(items[i]);
            return fin;
        }
    }
}