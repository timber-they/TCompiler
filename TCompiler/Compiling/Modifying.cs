#region

using System.Collections.Generic;
using System.Linq;
using TCompiler.Settings;

#endregion

namespace TCompiler.Compiling
{
    /// <summary>
    ///     Provides methods to modify the tCode
    /// </summary>
    /// <example>Adding the spaces</example>
    public static class Modifying
    {
        /// <summary>
        ///     Evaluates the code with all inserted spaces (For the interruptions)
        /// </summary>
        /// <param name="tCode"></param>
        /// <returns>The new tCode as a string</returns>
        private static string GetTCodeWithInsertedSpaces(string tCode)
        {
            char? previousChar = null;
            char? previousVisibleChar = null;
            var fin = "";
            var signs =
                GlobalProperties.AssignmentSigns.Concat(
                        GlobalProperties.OperationPriorities.Select(priority => priority.OperationSign))
                    .Concat(new List<string> {"(", ")"})
                    .ToList();
            var currentLineTillThere = "";

            for (var index = 0; index < tCode.Length; index++)
            {
                var currentChar = tCode[index];
                var nextChar = index < tCode.Length - 1 ? (char?) tCode[index + 1] : null;

                var replaced = false;

                foreach (var sign in signs)
                {
                    if (sign.Length == 1 &&
                        currentChar == sign.FirstOrDefault() &&
                        (currentChar != '-' ||
                         (previousChar == null || previousChar == ']' || previousChar == ')' ||
                          !char.IsSymbol(previousChar.Value) && !char.IsPunctuation(previousChar.Value)) &&
                         (nextChar == null || nextChar == '[' || nextChar == '(' ||
                          !char.IsSymbol(nextChar.Value) && !char.IsPunctuation(nextChar.Value))) &&
                        signs.All(priority => sign.FirstOrDefault() != previousVisibleChar) &&
                        (currentChar != ':' && currentChar != '.' ||
                         GlobalProperties.AssignmentSigns.Any(s => currentLineTillThere.Contains(s))))
                    {
                        fin += $" {currentChar} ";
                        currentLineTillThere += currentChar.ToString();
                        replaced = true;
                        break;
                    }
                    if (sign.Length != 2 || nextChar == null ||
                        currentChar != sign[0] ||
                        nextChar.Value != sign[1])
                        continue;

                    fin += $" {currentChar}{nextChar} ";
                    currentLineTillThere += currentChar.ToString() + nextChar;
                    index++;
                    replaced = true;
                    break;
                }

                if (!replaced)
                {
                    fin += currentChar.ToString();
                    if (currentChar == '\n')
                        currentLineTillThere = "";
                    else
                        currentLineTillThere += currentChar.ToString();
                }

                previousChar = currentChar;
                if (!char.IsWhiteSpace(currentChar))
                    previousVisibleChar = currentChar;
            }
            return fin;
        }

        /// <summary>
        ///     Removes the comments from the code
        /// </summary>
        /// <param name="tCode">The code to remove the comments from</param>
        /// <returns>The assembler code to execte as a string</returns>
        private static string RemoveComments(string tCode)
            => string.Join("\n", tCode.Split('\n').Select(s => string.Join("", s.TakeWhile(c => c != ';')).Trim()));

        /// <summary>
        ///     Returns the modified tCode with all modifications applied
        /// </summary>
        /// <param name="tCode">The code to modify</param>
        /// <returns>The assembler code to execute as a string</returns>
        public static string GetModifiedTCode(string tCode) => GetTCodeWithInsertedSpaces(RemoveComments(tCode));
    }
}