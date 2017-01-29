using System;
using System.Collections.Generic;
using System.Linq;
using TCompiler.Settings;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes.TemporaryOperation.TemporaryReturning;

namespace TCompiler.Types.CompilingTypes.TemporaryOperation.TemporaryParsedStringOperation
{
    public class TemporaryParsedStringOperation
    {
        public TemporaryParsedStringOperation(string tLine)
        {
            Items = new List<TemporaryParsedStringOperationItem>();
            foreach (var s in tLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                switch (s)
                {
                    case "(":
                        Items.Add(new OpeningBracket());
                        break;
                    case ")":
                        Items.Add(new ClosingBracket());
                        break;
                    default:
                        var operationSign =
                            GlobalProperties.OperationPriorities.FirstOrDefault(
                                priority => priority.OperationSign.Equals(s));
                        Items.Add(operationSign != null
                            ? (TemporaryParsedStringOperationItem) new OperationSign(s)
                            : new TemporaryVariableConstantMethodCall(s));
                        break;
                }
            }
        }

        private TemporaryParsedStringOperation(List<TemporaryParsedStringOperationItem> items)
        {
            Items = items;
        }

        public Tuple<int, ITemporaryReturning> GeTemporaryReturning()
        {
            if (Items.All(item => !(item is OperationSign)))
            {
                if (Items.Count != 1)
                    throw new ParameterException(GlobalProperties.LineIndex, Items.LastOrDefault()?.Value ?? "?");
                if (Items.First() is TemporaryVariableConstantMethodCall)
                    return new Tuple<int, ITemporaryReturning>(Items.Count,
                        new TemporaryVariableConstantMethodCallOrNothing(Items.First().Value));
                throw new ParameterException(GlobalProperties.LineIndex, Items.First().Value);
            }

            var fin = new TemporaryReturning.TemporaryOperation();
            var count = 0;

            for (var i = Items.Count - 1; i >= 0; i--)
            {
                count++;
                var item = Items[i];
                if (item is Bracket)
                {
                    if (item is OpeningBracket)
                        return new Tuple<int, ITemporaryReturning>(count, string.IsNullOrEmpty(fin.Sign) ? fin.B : fin);
                    var b = new TemporaryParsedStringOperation(Items.GetRange(0, i)).GeTemporaryReturning();
                    fin.B = b.Item2;
                    count += b.Item1;
                    i -= b.Item1;
                }
                else if (item is TemporaryVariableConstantMethodCall)
                    fin.B = new TemporaryVariableConstantMethodCallOrNothing(item.Value);
                else if (item is OperationSign)
                {
                    fin.Sign = item.Value;
                    var a = new TemporaryParsedStringOperation(Items.GetRange(0, i)).GeTemporaryReturning();
                    fin.A = a.Item2;
                    count += a.Item1;
                    if (fin.B != null && fin.A != null)
                        return new Tuple<int, ITemporaryReturning>(count, fin);
                    
                    throw new ParameterException(GlobalProperties.LineIndex, item.Value);
                }
            }
            if (fin.B != null && fin.A != null)
                return new Tuple<int, ITemporaryReturning>(Items.Count,
                    fin.B);

            throw new ParameterException(GlobalProperties.LineIndex, Items.LastOrDefault()?.Value ?? "?");
        }

        private List<TemporaryParsedStringOperationItem> Items { get; }
    }
}