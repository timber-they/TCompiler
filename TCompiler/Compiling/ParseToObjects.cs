using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using TCompiler.Enums;
using TCompiler.Types.CompilingTypes;
using TCompiler.Types.CompilingTypes.Block;
using TCompiler.Types.CompilingTypes.ReturningCommand.Method;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;
using Char = TCompiler.Types.CompilingTypes.ReturningCommand.Variable.Char;

namespace TCompiler.Compiling
{
    public static class ParseToObjects
    {
        private static List<Label> _labelList = new List<Label>();
        private static List<Block> _blockList = new List<Block>();
        private static List<Variable> _variableList = new List<Variable>();
        private static List<Method> _mehtodList = new List<Method>();

        public static List<Command> ParseTCodeToCommands(string tCode)
        {
            tCode = tCode.ToLower();
            var splitted = tCode.Split('\n').Select(s => s.Trim());
            var fin = new List<Command>();

            foreach (var tLine in splitted)
            {
                var type = GetCommandType(tLine);
                switch (type)
                {
                    case CommandType.VariableConstantMethodCallOrNothing:
                        {
                            var method = GetMethod(tLine);
                            if (method != null)
                            {
                                fin.Add(new MethodCall(method));
                                break;
                            }

                            var variable = GetVariable(tLine);
                            if (variable != null)
                            {
                                fin.Add(new VariableCall(variable));
                                break;
                            }

                            bool b;
                            if (bool.TryParse(tLine, out b))
                            {
                                fin.Add(new Bool(true, null, b));
                                break;
                            }

                            int i;
                            if (int.TryParse(tLine, NumberStyles.Integer, CultureInfo.CurrentCulture, out i))
                            {
                                fin.Add(new Cint(true, null, (byte)Convert.ToSByte(i)));
                                break;
                            }

                            uint ui;
                            if (uint.TryParse(tLine, 0 << 1, CultureInfo.CurrentCulture, out ui))
                            {
                                fin.Add(new Int(true, null, Convert.ToByte(ui)));
                                break;
                            }

                            char c;
                            if (tLine.StartsWith("'") && tLine.EndsWith("'") && char.TryParse(tLine.Trim('\''), out c))
                                fin.Add(new Types.CompilingTypes.ReturningCommand.Variable.Char(true, null, (byte)c));

                            break;
                        }
                    case CommandType.Block:         //TODO
                        {
                            break;
                        }
                    case CommandType.IfBlock:       //TODO
                        {
                            break;
                        }
                    case CommandType.WhileBlock:    //TODO
                        {
                            break;
                        }
                    case CommandType.ForTil:        //TODO
                        {
                            break;
                        }
                    case CommandType.Break:         //TODO
                        {
                            break;
                        }
                    case CommandType.Method:        //TODO
                        {
                            break;
                        }
                    case CommandType.Return:        //TODO
                        {
                            break;
                        }
                    case CommandType.And:
                        {
                            fin.Add(new Add(GetParametersWithDivider('&', tLine)));
                            break;
                        }
                    case CommandType.Not:
                        {
                            fin.Add(GetParameter('!', tLine));
                            break;
                        }
                    case CommandType.Or:
                        {
                            fin.Add(new Add(GetParametersWithDivider('|', tLine)));
                            break;
                        }
                    case CommandType.Add:
                        {
                            fin.Add(new Add(GetParametersWithDivider('+', tLine)));
                            break;
                        }
                    case CommandType.Subtract:
                        {
                            fin.Add(new Add(GetParametersWithDivider('-', tLine)));
                            break;
                        }
                    case CommandType.Multiply:
                        {
                            fin.Add(new Add(GetParametersWithDivider('*', tLine)));
                            break;
                        }
                    case CommandType.Divide:
                        {
                            fin.Add(new Add(GetParametersWithDivider('/', tLine)));
                            break;
                        }
                    case CommandType.Modulo:
                        {
                            fin.Add(new Add(GetParametersWithDivider('%', tLine)));
                            break;
                        }
                    case CommandType.Assignment:    //TODO
                        {
                            break;
                        }
                    case CommandType.Bool:
                        {
                            var b = new Bool(false, GetVariableDefinitionName(tLine));
                            fin.Add(b);
                            _variableList.Add(b);
                            break;
                        }
                    case CommandType.Char:
                        {
                            var c = new Char(false, GetVariableDefinitionName(tLine));
                            fin.Add(c);
                            _variableList.Add(c);
                            break;
                        }
                    case CommandType.Int:
                        {
                            var i = new Int(false, GetVariableDefinitionName(tLine));
                            fin.Add(i);
                            _variableList.Add(i);
                            break;
                        }
                    case CommandType.Cint:
                        {
                            var ci = new Cint(false, GetVariableDefinitionName(tLine));
                            fin.Add(ci);
                            _variableList.Add(ci);
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return fin;
        }

        private static CommandType GetCommandType(string tLine)
        {
            switch (tLine.Split(' ').FirstOrDefault())
            {
                case "int":
                    return CommandType.Int;
                case "if":
                    return CommandType.IfBlock;
                case "bool":
                    return CommandType.Bool;
                case "do":
                case "while":
                    return CommandType.WhileBlock;
                case "break":
                    return CommandType.Break;
                case "block":
                    return CommandType.Block;
                case "fortil":
                    return CommandType.ForTil;
                case "cint":
                    return CommandType.Cint;
                case "char":
                    return CommandType.Char;
                case "return":
                    return CommandType.Return;
                case "method":
                    return CommandType.Method;
                default:
                    return tLine.Contains(":=")
                        ? CommandType.Assignment
                        : (tLine.Contains("&")
                            ? CommandType.And
                            : (tLine.Contains("|")
                                ? CommandType.Or
                                : (tLine.Contains("!")
                                    ? CommandType.Not
                                    : (tLine.Contains("+")
                                        ? CommandType.Add
                                        : (tLine.Contains("-")
                                            ? CommandType.Subtract
                                            : (tLine.Contains("*")
                                                ? CommandType.Multiply
                                                : (tLine.Contains("/")
                                                    ? CommandType.Divide
                                                    : (tLine.Contains("%")
                                                        ? CommandType.Modulo
                                                        : CommandType.VariableConstantMethodCallOrNothing))))))));
            }
        }

        private static VariableType GetVariableType(string variableName)
        {
            VariableType fin;
            return
                Enum.TryParse(
                    _variableList.FirstOrDefault(
                        variable =>
                            string.Equals(variable.Name, variableName,
                                StringComparison.CurrentCultureIgnoreCase))?.GetType().Name, out fin)
                    ? fin
                    : VariableType.Nothing;
        }

        private static Variable GetVariable(string variableName)
            =>
            _variableList.FirstOrDefault(
                variable => string.Equals(variable.Name, variableName, StringComparison.CurrentCultureIgnoreCase));

        private static Method GetMethod(string methodName)
            =>
            _mehtodList.FirstOrDefault(
                method => string.Equals(method.Name, methodName, StringComparison.CurrentCultureIgnoreCase));

        private static Tuple<Variable, Variable> GetParametersWithDivider(char divider, string line)
        {
            var ss = line.Split(divider);
            return new Tuple<Variable, Variable>(GetVariable(ss[0]), GetVariable(ss[1]));
        }

        private static Variable GetParameter(char divider, string line) => GetVariable(line.Trim(divider));

        private static string GetVariableDefinitionName(string line) => line.Split(' ')[1];
    }
}