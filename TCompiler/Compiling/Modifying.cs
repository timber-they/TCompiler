#region

using System.Collections.Generic;
using System.Linq;

using TCompiler.Settings;
using TCompiler.Types.CompilerTypes;

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
        private static List <CodeLine> GetTCodeWithInsertedSpaces (List <CodeLine> tCode)
        {
            char? previousChar        = null;
            char? previousVisibleChar = null;
            var   fin                 = new List <CodeLine> ();
            var signs =
                GlobalProperties.AssignmentSigns.Concat (
                                     GlobalProperties.OperationPriorities.Select (priority => priority.OperationSign)).
                                 Concat (new List <string> {"(", ")", "[", "]"}).ToList ();
            var currentLineTillThere = "";

            foreach (var line in tCode)
            {
                var finLine = "";
                for (var index = 0; index < line.Line.Length; index++)
                {
                    var currentChar = line.Line [index];
                    var nextChar    = index < line.Line.Length - 1 ? (char?) line.Line [index + 1] : null;

                    var replaced = false;

                    foreach (var sign in signs)
                    {
                        if (sign.Length == 1 &&
                            currentChar == sign.FirstOrDefault () &&
                            (currentChar != '-' ||
                             (previousChar == null ||
                              previousChar == ']' ||
                              previousChar == ')' ||
                              !char.IsSymbol (previousChar.Value) && !char.IsPunctuation (previousChar.Value)) &&
                             (nextChar == null ||
                              nextChar == '[' ||
                              nextChar == '(' ||
                              !char.IsSymbol (nextChar.Value) && !char.IsPunctuation (nextChar.Value))) &&
                            signs.All (priority => sign.FirstOrDefault () != previousVisibleChar) &&
                            (currentChar != ':' && currentChar != '.' ||
                             GlobalProperties.AssignmentSigns.Any (s => currentLineTillThere.Contains (s))))
                        {
                            finLine              += $" {currentChar} ";
                            currentLineTillThere += currentChar.ToString ();
                            replaced             =  true;
                            break;
                        }

                        if (sign.Length != 2 ||
                            nextChar == null ||
                            currentChar != sign [0] ||
                            nextChar.Value != sign [1])
                            continue;

                        finLine              += $" {currentChar}{nextChar} ";
                        currentLineTillThere += currentChar.ToString () + nextChar;
                        index++;
                        replaced = true;
                        break;
                    }

                    if (!replaced)
                    {
                        finLine += currentChar.ToString ();
                        if (currentChar == '\n')
                            currentLineTillThere = "";
                        else
                            currentLineTillThere += currentChar.ToString ();
                    }

                    previousChar = currentChar;
                    if (!char.IsWhiteSpace (currentChar))
                        previousVisibleChar = currentChar;
                }

                fin.Add (new CodeLine (finLine.Trim (), line.FileName, line.LineIndex));
            }

            return fin;
        }

        /// <summary>
        ///     Removes the comments from the code
        /// </summary>
        /// <param name="tCode">The code to remove the comments from</param>
        /// <returns>The assembler code to execte as a string</returns>
        private static List <CodeLine> RemoveComments (List <CodeLine> tCode)
            => tCode.Select (t => new CodeLine (string.Join ("", t.Line.TakeWhile (c => c != ';')).Trim (), t.FileName,
                                                t.LineIndex)).ToList ();

        /// <summary>
        ///     Returns the modified tCode with all modifications applied
        /// </summary>
        /// <param name="tCode">The code to modify</param>
        /// <returns>The assembler code to execute as a string</returns>
        public static List <List <CodeLine>> GetModifiedTCode (IEnumerable <List <CodeLine>> tCode) =>
            tCode.Select (codeLines => GetTCodeWithInsertedSpaces (RemoveComments (codeLines))).ToList ();
    }
}