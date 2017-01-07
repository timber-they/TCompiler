#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCompiler.Enums;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes;
using TCompiler.Types.CompilingTypes.Block;
using TCompiler.Types.CompilingTypes.ReturningCommand.Method;

#endregion

namespace TCompiler.Compiling
{
    public static class ParseToAssembler
    {
        public static int LabelCount;
        private static int _line;

        public static Label Label
        {
            get
            {
                LabelCount++;
                return new Label($"l{LabelCount}");
            }
        }

        public static string ParseObjectsToAssembler(IEnumerable<Command> commands)
        {
            _line = 0;
            var fin = new StringBuilder();
            fin.AppendLine("include reg8051.inc");

            foreach (var command in commands)
            {
                var t = command.GetType();
                CommandType ct;
                if (Enum.TryParse(t.Name, true, out ct))
                    switch (ct)
                    {
                        case CommandType.Block:
                            break;
                        case CommandType.EndBlock:
                        {
                            var eb = (EndBlock) command;
                            var bt = eb.Block.GetType();

                            if (bt == typeof(WhileBlock))
                                fin.AppendLine($"jmp {((WhileBlock) eb.Block).UpperLabel}");
                            else if (bt == typeof(ForTilBlock))
                                fin.AppendLine(
                                    $"djnz {((ForTilBlock) eb.Block).Variable}, {((ForTilBlock) eb.Block).UpperLabel}");

                            fin.AppendLine(eb.Block.EndLabel.Name + ":");
                            break;
                        }
                        case CommandType.IfBlock:
                        {
                            var ib = (IfBlock) command;
                            fin.AppendLine(ib.Condition.ToString());
                            fin.AppendLine($"jnb acc.0, {ib.EndLabel}");
                            break;
                        }
                        case CommandType.WhileBlock:
                        {
                            var wb = (WhileBlock) command;
                            fin.AppendLine(wb.UpperLabel.LabelMark());
                            fin.AppendLine(wb.Condition.ToString());
                            fin.AppendLine($"jnb acc.0, {wb.EndLabel}");
                            break;
                        }
                        case CommandType.ForTilBlock:
                        {
                            var ftb = (ForTilBlock) command;
                            fin.AppendLine(ftb.Limit.ToString());
                            fin.AppendLine($"mov {ftb.Variable}, A");
                            fin.AppendLine($"{ftb.UpperLabel.LabelMark()}");
                            break;
                        }
                        case CommandType.Break:
                        {
                            var b = (Break) command;
                            fin.AppendLine($"jmp {b.CurrentBlock.EndLabel}");
                            break;
                        }
                        case CommandType.Method:
                        {
                            var m = (Method) command;
                            fin.AppendLine($"{m.Label.LabelMark()}");
                            break;
                        }
                        case CommandType.EndMethod:
                            fin.AppendLine("ret");
                            break;
                        case CommandType.Return:
                        case CommandType.MethodCall:
                        case CommandType.And:
                        case CommandType.Not:
                        case CommandType.Or:
                        case CommandType.Add:
                        case CommandType.Subtract:
                        case CommandType.Multiply:
                        case CommandType.Divide:
                        case CommandType.Modulo:
                        case CommandType.Assignment:
                        case CommandType.AddAssignment:
                        case CommandType.SubtractAssignment:
                        case CommandType.MultiplyAssignment:
                        case CommandType.DivideAssignment:
                        case CommandType.ModuloAssignment:
                        case CommandType.AndAssignment:
                        case CommandType.OrAssignment:
                        case CommandType.VariableCall:
                        case CommandType.ByteVariableCall:
                        case CommandType.BitVariableCall:
                        case CommandType.Bigger:
                        case CommandType.Smaller:
                        case CommandType.Equal:
                        case CommandType.UnEqual:
                        case CommandType.Increment:
                        case CommandType.Decrement:
                        case CommandType.ShiftLeft:
                        case CommandType.ShiftRight:
                        case CommandType.BitOf:
                        case CommandType.Declaration:
                            fin.AppendLine(command.ToString());
                            break;
                        case CommandType.Bool:
                        case CommandType.Char:
                        case CommandType.Int:
                        case CommandType.Cint: //Actually this will never happen again.
                            break;
                        case CommandType.Label: //TODO lol, I don't even have gotos
                            fin.AppendLine(((Label) command).LabelMark());
                            break;
                        case CommandType.Sleep:
                            var ranges = GetLoopRanges(((Sleep) command).TimeMs.Variable.Value);
                            var registers = new List<string>();
                            for (var i = 0; i < ranges.Count; i++)
                                registers.Add(ParseToObjects.CurrentRegister);
                            fin.AppendLine(GetAssemblerLoopLines(ranges, registers));
                            for (var i = 0; i < ranges.Count; i++)
                                ParseToObjects.CurrentRegisterAddress--;
                            break;
                        case CommandType.Empty:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                else
                    throw new Exception("Well Timo, you named your Classes differently to your Enum items.");
                _line++;
            }

            fin.AppendLine("end");
            var f =
                string.Join("\n", fin.ToString().Split('\n').Where(s => !string.IsNullOrEmpty(s.Trim('\r')))).ToUpper();
            return f.Substring(0, f.Last() == '\n' ? f.Length - 2 : f.Length - 1);
        }

        private static string GetAssemblerLoopLines(IReadOnlyCollection<int> loopRanges, IReadOnlyList<string> registers)
        {
            if (!loopRanges.Any())
                return string.Empty;

            var fin = new StringBuilder();
            var cl = Label;
            fin.AppendLine($"mov {registers[0]}, {loopRanges.Last()}");
            fin.AppendLine(cl.LabelMark());
            var lines = GetAssemblerLoopLines(loopRanges.Where((i, i1) => i1 < loopRanges.Count - 1).ToList(),
                registers.Where((s, i) => i != 0).ToList());
            if (!string.IsNullOrEmpty(lines))
                fin.AppendLine(lines);
            fin.AppendLine($"djnz {registers[0]}, {cl}");
            return fin.ToString();
        }

        private static List<int> GetLoopRanges(int time, int tolerance = 10) //time is in ms
        {
            time = (int) (time*921.583);
            var loopCount = 1;
            var fin = new List<int>();

            if (time == 0) return fin;
            for (var i = 0; i < loopCount; i++)
            {
                var ps = GetAllPossibilities(loopCount);
                var fod = ps.FirstOrDefault(ints => Math.Abs(GetTime(ints) - time) <= tolerance);
                if (fod != null)
                    return fod;
                loopCount++;
                if (loopCount > time)
                    throw new InvalidSleepTimeException(_line, time);
            }

            return fin;
        }

        private static int GetTime(IEnumerable<int> lC) => lC.Aggregate(0, (current, t) => (current + 2)*t + 1);

        private static IEnumerable<List<int>> GetAllPossibilities(int leftCount, int max = 255, int min = 1)
        {
            var fin = new List<List<int>>();
            for (var i = min; i < max; i++)
                if (leftCount > 0)
                    fin.AddRange(GetAllPossibilities(leftCount - 1).Select(possibility =>
                    {
                        possibility.Insert(0, i);
                        return possibility;
                    }));
                else
                    fin.Add(new List<int> {i});
            return fin;
        }
    }
}