using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using TCompiler.Enums;
using TCompiler.Types.CompilingTypes;
using TCompiler.Types.CompilingTypes.Block;
using TCompiler.Types.CompilingTypes.ReturningCommand;
using TCompiler.Types.CompilingTypes.ReturningCommand.Method;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;
using Char = TCompiler.Types.CompilingTypes.ReturningCommand.Variable.Char;

namespace TCompiler.Compiling
{
    public static class ParseToObjects
    {
        private static List<Label> _labelList = new List<Label>();
        private static readonly List<Block> BlockList = new List<Block>();
        private static readonly List<Variable> VariableList = new List<Variable>();
        private static readonly List<Method> MehtodList = new List<Method>();
        private static int _labelCount;

        private static int LabelCount
        {
            get
            {
                _labelCount++;
                return _labelCount;
            }
            set { _labelCount = value; }
        }

        public static List<Command> ParseTCodeToCommands(string tCode)
        {
            LabelCount = -1;
            tCode = tCode.ToLower();
            var splitted = tCode.Split('\n').Select(s => s.Trim()).ToArray();
            var fin = new List<Command>();

            foreach (var tLine in splitted)
            {
                var type = GetCommandType(tLine);
                switch (type)
                {
                    case CommandType.VariableConstantMethodCallOrNothing:
                        {
                            var vcmn = GetVariableConstantMethodCallOrNothing(tLine);
                            if (vcmn != null)
                                fin.Add(vcmn);
                            break;
                        }
                    case CommandType.Block:
                        {
                            var b = new Block(null);
                            BlockList.Add(b);
                            fin.Add(b);
                            break;
                        }
                    case CommandType.EndForTil:
                    case CommandType.EndWhile:
                    case CommandType.EndIf:
                    case CommandType.EndBlock:
                        {
                            BlockList.Last().EndLabel = new Label($"l{LabelCount}");
                            break;
                        }
                    case CommandType.IfBlock:
                        {
                            var b = new IfBlock(null, GetCondition(tLine));
                            BlockList.Add(b);
                            fin.Add(b);
                            break;
                        }
                    case CommandType.WhileBlock:
                        {
                            var b = new WhileBlock(null, GetCondition(tLine));
                            BlockList.Add(b);
                            fin.Add(b);
                            break;
                        }
                    case CommandType.ForTil:
                        {
                            var b = new ForTilBlock(null, GetParameterForTil(tLine));
                            BlockList.Add(b);
                            fin.Add(b);
                            break;
                        }
                    case CommandType.Break:
                        {
                            fin.Add(new Break(BlockList.Last().EndLabel));
                            break;
                        }
                    case CommandType.Method:
                        {
                            var m = new Method(tLine.Split(' ')[1]);
                            MehtodList.Add(m);
                            fin.Add(m);
                            break;
                        }
                    case CommandType.EndMethod:
                        {
                            fin.Add(new EndMethod());
                            break;
                        }
                    case CommandType.Return:
                        {
                            fin.Add(new Return(GetReturningCommand(tLine.Split()[1])));
                            break;
                        }
                    case CommandType.And:
                    case CommandType.Not:
                    case CommandType.Or:
                    case CommandType.Add:
                    case CommandType.Subtract:
                    case CommandType.Multiply:
                    case CommandType.Divide:
                    case CommandType.Modulo:
                        {
                            fin.Add(GetOperation(type, tLine));
                            break;
                        }
                    case CommandType.Assignment:
                        {
                            var pars = GetAssignmentParameter(tLine);
                            fin.Add(new Assignment(pars.Item1, pars.Item2));
                            break;
                        }
                    case CommandType.Bool:
                        {
                            var b = new Bool(false, GetVariableDefinitionName(tLine));
                            fin.Add(b);
                            VariableList.Add(b);
                            break;
                        }
                    case CommandType.Char:
                        {
                            var c = new Char(false, GetVariableDefinitionName(tLine));
                            fin.Add(c);
                            VariableList.Add(c);
                            break;
                        }
                    case CommandType.Int:
                        {
                            var i = new Int(false, GetVariableDefinitionName(tLine));
                            fin.Add(i);
                            VariableList.Add(i);
                            break;
                        }
                    case CommandType.Cint:
                        {
                            var ci = new Cint(false, GetVariableDefinitionName(tLine));
                            fin.Add(ci);
                            VariableList.Add(ci);
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return fin;
        }

        private static Operation GetOperation(CommandType ct, string line)
        {
            switch (ct)
            {
                case CommandType.And:
                    return new Add(GetParametersWithDivider('&', line));
                case CommandType.Not:
                    return new Not(GetParameter('!', line));
                case CommandType.Or:
                    return new Add(GetParametersWithDivider('|', line));
                case CommandType.Add:
                    return new Add(GetParametersWithDivider('+', line));
                case CommandType.Subtract:
                    return new Add(GetParametersWithDivider('-', line));
                case CommandType.Multiply:
                    return new Add(GetParametersWithDivider('*', line));
                case CommandType.Divide:
                    return new Add(GetParametersWithDivider('/', line));
                case CommandType.Modulo:
                    return new Add(GetParametersWithDivider('%', line));
                default:
                    return null;
            }
        }

        private static Command GetVariableConstantMethodCallOrNothing(string tLine)
        {
            var method = GetMethod(tLine);
            if (method != null)
                return new MethodCall(method);

            var variable = GetVariable(tLine);
            if (variable != null)
                return new VariableCall(variable);

            bool b;
            if (bool.TryParse(tLine, out b))
                return new Bool(true, null, b);

            int i;
            if (int.TryParse(tLine, NumberStyles.Integer, CultureInfo.CurrentCulture, out i))
                return new Cint(true, null, (byte)Convert.ToSByte(i));

            uint ui;
            if (uint.TryParse(tLine, 0 << 1, CultureInfo.CurrentCulture, out ui))
                return new Int(true, null, Convert.ToByte(ui));

            char c;
            return tLine.StartsWith("'") && tLine.EndsWith("'") && char.TryParse(tLine.Trim('\''), out c)
                ? new Char(true, null, (byte)c)
                : null;
        }

        private static ByteVariable GetParameterForTil(string line) => GetVariableConstantMethodCallOrNothing(line) as ByteVariable;

        private static CommandType GetCommandType(string tLine)
        {
            switch (tLine.Split(' ').FirstOrDefault())
            {
                case "int":
                    return CommandType.Int;
                case "if":
                    return CommandType.IfBlock;
                case "endif":
                    return CommandType.EndIf;
                case "bool":
                    return CommandType.Bool;
                case "while":
                    return CommandType.WhileBlock;
                case "endwhile":
                    return CommandType.EndWhile;
                case "break":
                    return CommandType.Break;
                case "block":
                    return CommandType.Block;
                case "endblock":
                    return CommandType.EndBlock;
                case "fortil":
                    return CommandType.ForTil;
                case "endfortil":
                    return CommandType.EndForTil;
                case "cint":
                    return CommandType.Cint;
                case "char":
                    return CommandType.Char;
                case "return":
                    return CommandType.Return;
                case "method":
                    return CommandType.Method;
                case "endmethod":
                    return CommandType.EndMethod;
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
                    VariableList.FirstOrDefault(
                        variable =>
                            string.Equals(variable.Name, variableName,
                                StringComparison.CurrentCultureIgnoreCase))?.GetType().Name, out fin)
                    ? fin
                    : VariableType.Nothing;
        }

        private static Variable GetVariable(string variableName)
            =>
            VariableList.FirstOrDefault(
                variable => string.Equals(variable.Name, variableName, StringComparison.CurrentCultureIgnoreCase));

        private static Method GetMethod(string methodName)
            =>
            MehtodList.FirstOrDefault(
                method => string.Equals(method.Name, methodName, StringComparison.CurrentCultureIgnoreCase));

        private static Tuple<Variable, Variable> GetParametersWithDivider(char divider, string line)
        {
            var ss = line.Split(divider).Select(s => s.Trim()).ToArray();
            return new Tuple<Variable, Variable>(GetVariable(ss[0]), GetVariable(ss[1]));
        }

        private static Variable GetParameter(char divider, string line) => GetVariable(line.Trim(divider));

        private static string GetVariableDefinitionName(string line) => line.Split(' ')[1];

        private static Condition GetCondition(string line)
        {
            var sb = new StringBuilder();
            foreach (var s in line.Split('[')[1])
            {
                if (s == ']')
                    break;
                sb.Append(s);
            }
            return new Condition(GetReturningCommand(sb.ToString()));
        }

        private static Tuple<Variable, ReturningCommand> GetAssignmentParameter(string tLine)
        {
            var splitted = Split(tLine, ":=").Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
            return new Tuple<Variable, ReturningCommand>(GetVariable(splitted[0]), GetReturningCommand(splitted[1]));
        }

        private static IEnumerable<string> Split(string toSplit, string splitter)
        {
            var fin = new List<string>();
            var sb = new StringBuilder();
            var ts = toSplit.ToCharArray();
            var s = splitter.ToCharArray();

            for (var i = 0; i < ts.Length - (s.Length - 1); i++)
            {
                if (s.Where((t, j) => t != ts[i + j]).Any())
                {
                    sb.Append(ts[i]);
                    continue;
                }
                fin.Add(sb.ToString());
                sb = new StringBuilder();
                i += splitter.Length;
            }
            return fin;
        }

        private static ReturningCommand GetReturningCommand(string line)
            =>
            GetVariableConstantMethodCallOrNothing(line) as ReturningCommand ?? GetOperation(GetCommandType(line), line);
    }
}