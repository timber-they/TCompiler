#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace TIDE.Coloring.StringFunctions
{
    /// <summary>
    ///     Provides some string functions
    /// </summary>
    public static class StringFunctions
    {
        /// <summary>
        ///     Evaluates the chars that got removed from before to after
        /// </summary>
        /// <param name="before">The string like it was before</param>
        /// <param name="after">The string like it is after</param>
        /// <returns>The stuff that got removed as a list of chars</returns>
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

        /// <summary>
        ///     Evaluates the chars that got added from before to after
        /// </summary>
        /// <param name="before">The string before the changes happened</param>
        /// <param name="after">The string after the changes happened</param>
        /// <returns>The characters as a list of chars</returns>
        public static List<char> GetAdded(string before, string after)
        {
            var b = before.ToCharArray().ToList();
            var a = after.ToCharArray().ToList();

            foreach (var c in b)
            {
                if (!a.Contains(c))
                    return new List<char>();
                a.Remove(c);
            }
            return a;
        }
    }
}