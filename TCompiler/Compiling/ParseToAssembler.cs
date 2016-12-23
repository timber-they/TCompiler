using System;
using System.Collections.Generic;
using System.Text;
using TCompiler.Enums;
using TCompiler.Types;
using TCompiler.Types.CompilingTypes;
using TCompiler.Types.CompilingTypes.Block;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Compiling
{
    public static class ParseToAssembler
    {
        private static int _byteCounter;
        private static IntPair _bitCounter;
        private static int Label2;

        private static int ByteCounter
        {
            get
            {
                _byteCounter++;
                return _byteCounter;
            }
            set { _byteCounter = value; }
        }

        private static IntPair BitCounter
        {
            get
            {
                IncreaseBitCounter();
                return _bitCounter;
            }
        }

        private static void IncreaseBitCounter()
        {
            if (BitCounter.Item2 < 7)
                BitCounter.Item2++;
            else
            {
                BitCounter.Item1++;
                BitCounter.Item2 = 0;
            }
        }

        private static void DecreaseBitCounter()
        {
            if (BitCounter.Item2 > 0)
                BitCounter.Item2--;
            else
            {
                BitCounter.Item1--;
                BitCounter.Item2 = 7;
            }
        }

        public static string Label1
        {
            get
            {
                Label2++;
                return $"l{Label2}";
            }
        }

        public static string ParseObjectsToAssembler(List<Command> commands, int labelCount)
        {
            Label2 = labelCount;
            var fin = new StringBuilder();

            foreach (var command in commands)
            {
                var t = command.GetType();
                CommandType ct;
                if (Enum.TryParse(t.Name, true, out ct))
                {
                    switch (ct)
                    {
                        case CommandType.Block:
                            break;
                        case CommandType.EndBlock:
                            {
                                var eb = (EndBlock)command;
                                var bt = eb.Block.GetType();

                                if (bt == typeof(WhileBlock))
                                    fin.AppendLine($"jmp {((WhileBlock)eb.Block).UpperLabel}");
                                else if (bt == typeof(ForTilBlock))
                                    fin.AppendLine($"djnz A, {((ForTilBlock)eb.Block).UpperLabel}");

                                fin.AppendLine(eb.Block.EndLabel.Name);
                                foreach (var variable in eb.Block.Variables)
                                {
                                    if (variable is ByteVariable)
                                        ByteCounter--;
                                    else
                                        DecreaseBitCounter();
                                }
                                break;
                            }
                        case CommandType.IfBlock:
                            {
                                var ib = (IfBlock)command;
                                fin.AppendLine(ib.Condition.ToString());
                                fin.AppendLine($"jnb acc.0, {ib.EndLabel}");
                                break;
                            }
                        case CommandType.WhileBlock:
                            {
                                var wb = (WhileBlock)command;
                                fin.AppendLine($"{wb.UpperLabel}:");
                                fin.AppendLine(wb.Condition.ToString());
                                fin.AppendLine($"jnb acc.0, {wb.EndLabel}");
                                break;
                            }
                        case CommandType.ForTilBlock:
                            {
                                var ftb = (ForTilBlock)command;
                                fin.AppendLine($"mov A, {ftb.Limit}");
                                fin.AppendLine($"{Label1}:");
                                break;
                            }
                        case CommandType.Break:
                            {
                                var b = (Break)command;
                                fin.AppendLine($"jmp {b.CurrentBlockEndLabel}");
                                break;
                            }
                        case CommandType.Method:
                            break;
                        case CommandType.EndMethod:
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
                        case CommandType.VariableCall:
                            fin.AppendLine(command.ToString());
                            break;
                        case CommandType.Bool:
                            break;
                        case CommandType.Char:
                            break;
                        case CommandType.Int:
                            break;
                        case CommandType.Cint:
                            break;
                        case CommandType.Label:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                    throw new Exception("Well Timo, you named your Classes differently to your EnumSpecs.");
            }

            return fin.ToString();
        }
    }
}