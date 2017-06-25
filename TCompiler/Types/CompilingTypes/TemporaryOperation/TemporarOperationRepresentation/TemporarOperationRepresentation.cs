#region

using System;
using System.Collections.Generic;
using System.Linq;

using TCompiler.Settings;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes.TemporaryOperation.TemporaryReturning;

#endregion


namespace TCompiler.Types.CompilingTypes.TemporaryOperation.TemporarOperationRepresentation
{
    /// <summary>
    ///     Represents an operation
    /// </summary>
    public class TemporarOperationRepresentation
    {
        /// <summary>
        ///     Initiates/Evaluates a new temporar operation representation
        /// </summary>
        /// <param name="tLine">The line of the representation to evaluate the rest</param>
        public TemporarOperationRepresentation (string tLine)
        {
            Items = new List<TemporarOperationItemRepresentation> ();
            foreach (var s in tLine.Split (new [] {' '}, StringSplitOptions.RemoveEmptyEntries))
                switch (s)
                {
                    case "(":
                        Items.Add (new OpeningTemporarBracketRepresentation ());
                        break;
                    case ")":
                        Items.Add (new ClosingTemporarBracketRepresentation ());
                        break;
                    default:
                        var operationSign =
                            GlobalProperties.OperationPriorities.FirstOrDefault (
                                priority => priority.OperationSign.Equals (s));
                        Items.Add (operationSign != null
                                       ? (TemporarOperationItemRepresentation)
                                       new TemporarOperationSignRepresentation (
                                           s, operationSign.LeftRightParameterRequired)
                                       : new TemporarVariableConstantMethodCallRepresentation (s));
                        break;
                }
        }

        /// <summary>
        ///     Initiates a new temporar opeartion representation with the list of the items
        /// </summary>
        /// <param name="items">The items (content) of the representation</param>
        private TemporarOperationRepresentation (List<TemporarOperationItemRepresentation> items) => Items = items;

        /// <summary>
        ///     The items (content) of the representation
        /// </summary>
        private List<TemporarOperationItemRepresentation> Items { get; }

        /// <summary>
        ///     Evaluates the TemporaryReturning stuff
        /// </summary>
        /// <returns>The stuff as a form of ITemporaryReturning</returns>
        public Tuple<int, ITemporaryReturning> GeTemporaryReturning ()
        {
            if (Items.All (item => !(item is TemporarOperationSignRepresentation)))
            {
                if (Items.Count != 1)
                    return null;
                if (Items.First () is TemporarVariableConstantMethodCallRepresentation)
                    return new Tuple<int, ITemporaryReturning> (Items.Count,
                                                                new TemporaryVariableConstantMethodCallOrNothing (
                                                                    Items.First ().Value));
                throw new ParameterException (GlobalProperties.CurrentLine, Items.First ().Value);
            }

            var fin = new TemporaryReturning.TemporaryOperation ();
            var count = 0;

            for (var i = Items.Count - 1; i >= 0; i--)
            {
                count++;
                var item = Items [i];
                if (item is TemporarBracketRepresentation)
                {
                    if (item is OpeningTemporarBracketRepresentation)
                        return new Tuple<int, ITemporaryReturning> (
                            count, string.IsNullOrEmpty (fin.Sign) ? fin.B : fin);
                    var b = new TemporarOperationRepresentation (Items.GetRange (0, i)).GeTemporaryReturning ();
                    fin.B = b.Item2;
                    count += b.Item1;
                    i -= b.Item1;
                }
                else if (item is TemporarVariableConstantMethodCallRepresentation)
                {
                    if (fin.B != null)
                        throw new ParameterException (GlobalProperties.CurrentLine, item.Value);
                    fin.B = new TemporaryVariableConstantMethodCallOrNothing (item.Value);
                }
                else if (item is TemporarOperationSignRepresentation)
                {
                    fin.Sign = item.Value;
                    var a = new TemporarOperationRepresentation (Items.GetRange (0, i)).GeTemporaryReturning ();
                    fin.A = a?.Item2;
                    count += a?.Item1 ?? 0;
                    if ((fin.B != null ||
                         !((TemporarOperationSignRepresentation) item).LeftRightParameterRequired.Item2) &&
                        (fin.A != null ||
                         !((TemporarOperationSignRepresentation) item).LeftRightParameterRequired.Item1))
                        return new Tuple<int, ITemporaryReturning> (count, fin);

                    throw new ParameterException (GlobalProperties.CurrentLine, item.Value);
                }
            }
            return new Tuple<int, ITemporaryReturning> (Items.Count,
                                                        fin.B);
        }
    }
}