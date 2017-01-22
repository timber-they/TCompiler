#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCompiler.AssembleHelp;
using TCompiler.Enums;
using TCompiler.Settings;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes;
using TCompiler.Types.CompilingTypes.Block;
using TCompiler.Types.CompilingTypes.ReturningCommand.Method;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Compiling
{
    /// <summary>
    ///     Provides the methods for parsing a list of objects to assembler
    /// </summary>
    public static class ParseToAssembler
    {
        private static List<InterruptType> _interruptExecutions;

        /// <summary>
        ///     The count of the current label
        /// </summary>
        /// <example>325</example>
        public static int LabelCount { private get; set; }

        /// <summary>
        ///     The current line
        /// </summary>
        public static int Line { get; private set; }

        /// <summary>
        ///     The current label
        /// </summary>
        /// <remarks>
        ///     At each view the labelCount is increased
        /// </remarks>
        /// <example>
        ///     The label name: L325
        /// </example>
        /// <value>The label as a Label</value>
        public static Label Label
        {
            get
            {
                LabelCount++;
                return new Label($"l{LabelCount}");
            }
        }

        /// <summary>
        ///     The count for the help labels
        /// </summary>
        public static int HelpLabelCount { get; set; }

        /// <summary>
        ///     Parses the objects to assembler code
        /// </summary>
        /// <param name="commands">The commands as CommandObjects</param>
        /// <param name="tCode">This is mainly for debugging so I can write the source code into the compiled code</param>
        /// <returns>The parsed assembler code</returns>
        public static string ParseObjectsToAssembler(IEnumerable<Command> commands, string[] tCode)
        {
            _interruptExecutions = new List<InterruptType>();
            Line = 0;
            var fin = new StringBuilder();
            var insertBefore = new StringBuilder();

            foreach (var command in commands)
            {
                var line = tCode[Line];
                var splitterCount = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
                if ((command.ExpectedSplitterLengths != null) &&
                    command.ExpectedSplitterLengths.All(i => i != splitterCount))
                    throw new InvalidSplitterLengthException(Line, splitterCount);
                fin.AppendLine("; " + line);
                if (command.DeactivateEa)
                    fin.AppendLine(AssembleCodePreviews.BeforeCommand(_interruptExecutions));
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
                                    fin.AppendLine($"jmp {((WhileBlock) eb.Block).UpperLabel.DestinationName}");
                                else if (bt == typeof(ForTilBlock))
                                    fin.AppendLine(
                                        $"djnz {((ForTilBlock) eb.Block).Variable}, {((ForTilBlock) eb.Block).UpperLabel}");

                                fin.AppendLine(eb.Block.EndLabel.LabelMark());
                                break;
                            }
                        case CommandType.IfBlock:
                            {
                                var ib = (IfBlock) command;
                                var destinationLabel = ib.Else?.ElseLabel ?? ib.EndLabel;
                                JumpToLabelWithCondition(ib.Condition, fin, destinationLabel);
                                break;
                            }
                        case CommandType.ElseBlock:
                            {
                                fin.AppendLine($"jmp {((ElseBlock) command).EndLabel}");
                                fin.AppendLine(((ElseBlock) command).ElseLabel.LabelMark());
                                break;
                            }
                        case CommandType.WhileBlock:
                            {
                                var wb = (WhileBlock) command;
                                fin.AppendLine(wb.UpperLabel.LabelMark());
                                JumpToLabelWithCondition(wb.Condition, fin, wb.EndLabel);
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
                                fin.AppendLine($"jmp {b.CurrentBlock.EndLabel.DestinationName}");
                                break;
                            }
                        case CommandType.Method:
                            {
                                fin.AppendLine($"{((Method) command).Label.LabelMark()}");
                                break;
                            }
                        case CommandType.InterruptServiceRoutine:
                            {
                                var isr = (InterruptServiceRoutine) command;
                                InsertBeforeIsr(insertBefore, isr);
                                fin.AppendLine($"{isr.Label.LabelMark()}");
                                _interruptExecutions.Add(isr.InterruptType);
                                switch (isr.InterruptType)
                                {
                                    case InterruptType.ExternalInterrupt0:
                                        fin.AppendLine("clr 088h.1");
                                        break;
                                    case InterruptType.ExternalInterrupt1:
                                        fin.AppendLine("clr 088h.3");
                                        break;
                                    case InterruptType.CounterInterrupt0:
                                    case InterruptType.TimerInterrupt0:
                                        fin.AppendLine("clr 088h.4");
                                        fin.AppendLine("clr 088h.5");
                                        fin.AppendLine($"mov 08Ah, #{isr.StartValue.Item1}");
                                        fin.AppendLine($"mov 08Ch, #{isr.StartValue.Item2}");
                                        fin.AppendLine("setb 088h.4");
                                        break;
                                    case InterruptType.CounterInterrupt1:
                                    case InterruptType.TimerInterrupt1:
                                        fin.AppendLine("clr 088h.6");
                                        fin.AppendLine("clr 088h.7");
                                        fin.AppendLine($"mov 08Bh, #{isr.StartValue.Item1}");
                                        fin.AppendLine($"mov 08Dh, #{isr.StartValue.Item2}");
                                        fin.AppendLine("setb 088h.6");
                                        break;
                                }
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
                        case CommandType.Label: //TODO LOL, I don't even have gotos, nor labels
                            fin.AppendLine(((Label) command).LabelMark());
                            break;
                        case CommandType.Sleep:
                            var ranges = GetLoopRanges(((Sleep) command).TimeMs);
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
                if (command.ActivateEa)
                    fin.AppendLine(AssembleCodePreviews.AfterCommand(_interruptExecutions));
                Line++;
            }


            fin.AppendLine(AssembleCodePreviews.After());
            var f =
                string.Join("\n", fin.ToString().Split('\n').Where(s => !string.IsNullOrEmpty(s.Trim('\r')))).ToUpper();
            var before =
                AssembleCodePreviews.Before(
                    _interruptExecutions.Contains(InterruptType.ExternalInterrupt0)
                        ? GlobalProperties.ExternalInterrupt0ExecutionName
                        : null,
                    _interruptExecutions.Contains(InterruptType.ExternalInterrupt1)
                        ? GlobalProperties.ExternalInterrupt1ExecutionName
                        : null,
                    _interruptExecutions.Any(
                        type => (type == InterruptType.CounterInterrupt0) || (type == InterruptType.TimerInterrupt0))
                        ? GlobalProperties.TimerCounterInterrupt0ExecutionName
                        : null,
                    _interruptExecutions.Any(
                        type => (type == InterruptType.CounterInterrupt1) || (type == InterruptType.TimerInterrupt1))
                        ? GlobalProperties.TimerCounterInterrupt1ExecutionName
                        : null, _interruptExecutions.Contains(InterruptType.CounterInterrupt0),
                    _interruptExecutions.Contains(InterruptType.CounterInterrupt1));
            before += insertBefore.ToString();
            return
                $"{before}" +
                $"{f.Substring(0, f.Last() == '\n' ? f.Length - 2 : f.Length - 1)}";
        }

        private static void JumpToLabelWithCondition(Condition condition, StringBuilder fin, Label label)
        {

            var var = (condition.Evaluation as BitVariableCall)?.BitVariable;
            if (var?.IsConstant == true)
            {
                if (!var.Value)
                    fin.AppendLine($"jmp {label.DestinationName}");
                return;
            }
            if (var != null)
            {
                fin.AppendLine($"jnb {var.Address}, {label}");
                return;
            }

            fin.AppendLine(condition.ToString());
            fin.AppendLine($"jnb 224.0, {label}");
        }

        /// <summary>
        /// Adds the stuff to insert before when an isr occurs to the insertBefore stringBuilder
        /// </summary>
        /// <param name="insertBefore">The stringBuilder</param>
        /// <param name="isr">The isr for which the stuff is added</param>
        private static void InsertBeforeIsr(StringBuilder insertBefore, InterruptServiceRoutine isr)
        {
            switch (isr.InterruptType)
            {
                case InterruptType.CounterInterrupt0:
                case InterruptType.TimerInterrupt0:
                    insertBefore.AppendLine($"mov 08Ah, #{isr.StartValue.Item1}");
                    insertBefore.AppendLine($"mov 08Ch, #{isr.StartValue.Item2}");
                    break;
                case InterruptType.CounterInterrupt1:
                case InterruptType.TimerInterrupt1:
                    insertBefore.AppendLine(isr.StartValue.ToString());
                    insertBefore.AppendLine($"mov 08Bh, #{isr.StartValue.Item1}");
                    insertBefore.AppendLine($"mov 08Dh, #{isr.StartValue.Item2}");
                    break;
            }
        }

        /// <summary>
        ///     The assembler code to the given loopRanges
        /// </summary>
        /// <param name="loopRanges">The loop ranges for the loops</param>
        /// <param name="registers">The registers needed for the loops</param>
        /// <returns>Recursively the assembler code as a string</returns>
        private static string GetAssemblerLoopLines(IReadOnlyCollection<int> loopRanges, IReadOnlyList<string> registers)
        {
            if (!loopRanges.Any())
                return string.Empty;

            var fin = new StringBuilder();
            var cl = Label;
            fin.AppendLine($"mov {registers[0]}, #{loopRanges.Last()}");
            fin.AppendLine(cl.LabelMark());
            var lines = GetAssemblerLoopLines(loopRanges.Where((i, i1) => i1 < loopRanges.Count - 1).ToList(),
                registers.Where((s, i) => i != 0).ToList());
            if (!string.IsNullOrEmpty(lines))
                fin.AppendLine(lines);
            fin.AppendLine($"djnz {registers[0]}, {cl}");
            return fin.ToString();
        }

        /// <summary>
        ///     A list of the ranges of the loops for the specified sleep time
        /// </summary>
        /// <param name="time">The time in milliseconds to wait</param>
        /// <param name="tolerance">The tolerance time in machine cycles</param>
        /// <returns>The list of the ranges</returns>
        private static List<int> GetLoopRanges(int time, int tolerance = 10)
        {
            time = (int) (time * 921.583);
            var loopCount = 1;
            var fin = new List<int>();

            if (time == 0)
                return fin;
            for (var i = 0; i < loopCount; i++)
            {
                var ps = GetAllPossibilities(loopCount);
                var firstOrDefault = ps.FirstOrDefault(ints => Math.Abs(GetTime(ints) - time) <= tolerance);
                if (firstOrDefault != null)
                    return firstOrDefault;
                loopCount++;
                if (loopCount > time)
                    throw new InvalidSleepTimeException(Line, time);
            }

            return fin;
        }

        /// <summary>
        ///     The time of the specified loop ranges
        /// </summary>
        /// <param name="lC">The loop ranges</param>
        /// <returns>The time in machine cycles</returns>
        private static int GetTime(IEnumerable<int> lC) => lC.Aggregate(0, (current, t) => (current + 2) * t + 1);

        /// <summary>
        ///     Recursively gets all the possibilities for the specified amount of loops
        /// </summary>
        /// <param name="leftCount">The amount of loops</param>
        /// <param name="max">The maximum amount of repeat time</param>
        /// <param name="min">The minimum amount of repeat time</param>
        /// <returns></returns>
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
                    fin.Add(new List<int> { i });
            return fin;
        }
    }
}