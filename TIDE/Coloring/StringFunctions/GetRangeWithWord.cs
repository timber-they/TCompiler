#region

using System.Collections.Generic;
using TIDE.Coloring.Types;

#endregion

namespace TIDE.Coloring.StringFunctions
{
    /// <summary>
    ///     Provides methods for finding the range with a word
    /// </summary>
    public static class GetRangeWithWord
    {
        /// <summary>
        ///     Evaluates the range with the word and a list of all words
        /// </summary>
        /// <param name="that">The word of which the range shall get evaluated</param>
        /// <param name="words">A list of the words</param>
        /// <returns>The range</returns>
        public static Range GetRangeWithWordSpaces(Word that, IReadOnlyList<string> words)
        {
            var start = 0;
            for (var i = 0; i < that.PositionInWordArray; i++)
                start += words[i].Length + 1;
            return new Range(start, start + words[that.PositionInWordArray].Length);
        }
    }
}