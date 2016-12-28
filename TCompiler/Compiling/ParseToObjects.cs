using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes;
using TCompiler.Types.CompilingTypes.Block;
using TCompiler.Types.CompilingTypes.ReturningCommand;
using TCompiler.Types.CompilingTypes.ReturningCommand.Method;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;
using Char = TCompiler.Types.CompilingTypes.ReturningCommand.Variable.Char;
using CommandType = TCompiler.Enums.CommandType;

namespace TCompiler.Compiling
{
    public static class ParseToObjects
    {
        private static readonly List<Block> BlockList = new List<Block>();
        private static readonly List<Variable> VariableList = new List<Variable>();
        private static readonly List<Method> MehtodList = new List<Method>();
        private static Method _currentMethod;
        public static int CurrentRegister1 = -1;

        public static string CurrentRegister
        {
            get
            {
                CurrentRegister1++;
                if(CurrentRegister1 > 9)
                    throw new TooManyRegistersException();
                return $"R{CurrentRegister1}";
            }
        }

        public static IEnumerable<Command> ParseTCodeToCommands(string tCode)
        {
            ParseToAssembler.LabelCount = -1;
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
                            else
                                throw new InvalidCommandException(tLine);
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
                            var l = new Label(ParseToAssembler.Label.ToString());
                            fin.Add(new EndBlock(BlockList.Last()));
                            BlockList.Last().EndLabel = l;
                            foreach (var variable in BlockList.Last().Variables)
                                VariableList.Remove(variable);

                            if (BlockList.Last() is ForTilBlock)
                                CurrentRegister1--;

                            BlockList.RemoveRange(BlockList.Count - 1, 1);
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
                            var b = new WhileBlock(null, GetCondition(tLine), new Label(ParseToAssembler.Label.ToString()));
                            BlockList.Add(b);
                            fin.Add(b);
                            break;
                        }
                    case CommandType.ForTilBlock:
                        {
                            var b = new ForTilBlock(null, GetParameterForTil(tLine), new Label(ParseToAssembler.Label.ToString()), new Int(false, CurrentRegister));
                            BlockList.Add(b);
                            fin.Add(b);
                            break;
                        }
                    case CommandType.Break:
                        {
                            fin.Add(new Break(BlockList.Last()));
                            break;
                        }
                    case CommandType.Method:
                        {
                            var m = new Method(tLine.Split(' ')[1]);
                            MehtodList.Add(m);
                            fin.Add(m);
                            _currentMethod = m;
                            break;
                        }
                    case CommandType.EndMethod:
                        {
                            fin.Add(new EndMethod(_currentMethod));
                            foreach (var variable in _currentMethod?.Variables)
                                VariableList.Remove(variable);
                            _currentMethod = null;
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
                    case CommandType.Equal:
                    case CommandType.Bigger:
                    case CommandType.Smaller:
                    case CommandType.UnEqual:
                    case CommandType.Increment:
                    case CommandType.Decrement:
                    case CommandType.ShiftLeft:
                    case CommandType.ShiftRight:
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
                            if (BlockList.Count > 0)
                                BlockList.Last().Variables.Add(b);
                            else
                                _currentMethod?.Variables.Add(b);

                            break;
                        }
                    case CommandType.Char:
                        {
                            var c = new Char(false, GetVariableDefinitionName(tLine));
                            fin.Add(c);
                            VariableList.Add(c);
                            if (BlockList.Count > 0)
                                BlockList.Last().Variables.Add(c);
                            else
                                _currentMethod?.Variables.Add(c);
                            break;
                        }
                    case CommandType.Int:
                        {
                            var i = new Int(false, GetVariableDefinitionName(tLine));
                            fin.Add(i);
                            VariableList.Add(i);
                            if (BlockList.Count > 0)
                                BlockList.Last().Variables.Add(i);
                            else
                                _currentMethod?.Variables.Add(i);
                            break;
                        }
                    case CommandType.Cint:
                        {
                            var ci = new Cint(false, GetVariableDefinitionName(tLine));
                            fin.Add(ci);
                            VariableList.Add(ci);
                            if (BlockList.Count > 0)
                                BlockList.Last().Variables.Add(ci);
                            else
                                _currentMethod?.Variables.Add(ci);
                            break;
                        }
                    case CommandType.Sleep:
                        {
                            fin.Add(new Sleep((ByteVariableCall) GetParameter("sleep", tLine)));
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
            Tuple<VariableCall, VariableCall> vars;
            switch (ct)
            {
                case CommandType.And:
                    return new And(GetParametersWithDivider('&', line));
                case CommandType.Not:
                    return new Not(GetParameter('!', line));
                case CommandType.Or:
                    return new Or(GetParametersWithDivider('|', line));
                case CommandType.Add:
                    vars = GetParametersWithDivider('+', line);
                    return new Add((ByteVariableCall)vars.Item1, (ByteVariableCall)vars.Item2);
                case CommandType.Subtract:
                    vars = GetParametersWithDivider('-', line);
                    return new Subtract((ByteVariableCall)vars.Item1, (ByteVariableCall)vars.Item2);
                case CommandType.Multiply:
                    vars = GetParametersWithDivider('*', line);
                    return new Multiply((ByteVariableCall)vars.Item1, (ByteVariableCall)vars.Item2);
                case CommandType.Divide:
                    vars = GetParametersWithDivider('/', line);
                    return new Divide((ByteVariableCall)vars.Item1, (ByteVariableCall)vars.Item2);
                case CommandType.Modulo:
                    vars = GetParametersWithDivider('%', line);
                    return new Modulo((ByteVariableCall)vars.Item1, (ByteVariableCall)vars.Item2);
                case CommandType.Bigger:
                    vars = GetParametersWithDivider('>', line);
                    return new Bigger((ByteVariableCall)vars.Item1, (ByteVariableCall)vars.Item2);
                case CommandType.Smaller:
                    vars = GetParametersWithDivider('<', line);
                    return new Smaller((ByteVariableCall)vars.Item1, (ByteVariableCall)vars.Item2);
                case CommandType.Equal:
                    vars = GetParametersWithDivider('=', line);
                    return new Equal((ByteVariableCall)vars.Item1, (ByteVariableCall)vars.Item2);
                case CommandType.UnEqual:
                    vars = GetParametersWithDivider("!=", line);
                    return new Equal((ByteVariableCall)vars.Item1, (ByteVariableCall)vars.Item2);
                case CommandType.Increment:
                    return new Increment((ByteVariableCall)GetParameter("++", line));
                case CommandType.Decrement:
                    return new Decrement((ByteVariableCall)GetParameter("--", line));
                case CommandType.ShiftLeft:
                    vars = GetParametersWithDivider("<<", line);
                    return new ShiftLeft((ByteVariableCall)vars.Item1, (ByteVariableCall)vars.Item2, CurrentRegister, ParseToAssembler.Label);
                case CommandType.ShiftRight:
                    vars = GetParametersWithDivider("<<", line);
                    return new ShiftRight((ByteVariableCall)vars.Item1, (ByteVariableCall)vars.Item2, CurrentRegister, ParseToAssembler.Label);
                default:
                    return null;
            }
        }

        private static Command GetVariableConstantMethodCallOrNothing(string tLine)
        {
            if(string.IsNullOrEmpty(tLine))
                return new Empty();

            var method = GetMethod(tLine);
            if (method != null)
                return new MethodCall(method);

            var variable = GetVariable(tLine);
            if (variable != null)
            {
                var byteVariable = variable as ByteVariable;
                if (byteVariable != null)
                    return new ByteVariableCall(byteVariable);
                return new BitVariableCall((BitVariable)variable);
            }

            bool b;
            if (bool.TryParse(tLine, out b))
                return new BitVariableCall(new Bool(true, null, b));

            uint ui;
            if (tLine.StartsWith("0x") &&
                uint.TryParse(Trim("0x",tLine), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out ui) ||
                uint.TryParse(tLine, NumberStyles.None, CultureInfo.CurrentCulture, out ui))
                return new ByteVariableCall(new Int(true, null, Convert.ToByte(ui)));

            int i;
            if (tLine.StartsWith("0x") && int.TryParse(Trim("0x", tLine), 0 << 9, CultureInfo.CurrentCulture, out i) ||
                int.TryParse(tLine, NumberStyles.Number, CultureInfo.CurrentCulture, out i))
                return new ByteVariableCall(new Cint(true, null, (byte) Convert.ToSByte(i)));

            char c;
            if (tLine.StartsWith("'") && tLine.EndsWith("'") && char.TryParse(tLine.Trim('\''), out c))
                return new ByteVariableCall(new Char(true, null, (byte) c));

            return null;
        }

        private static ByteVariableCall GetParameterForTil(string line)
        {
            var p = GetVariableConstantMethodCallOrNothing(line.Trim().Split(' ')[1]) as ByteVariableCall;
            if(p == null)
                throw new InvalidCommandException(line);
            return p;
        }

        public static CommandType GetCommandType(string tLine)
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
                    return CommandType.ForTilBlock;
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
                case "sleep":
                    return CommandType.Sleep;
                default:
                    return tLine.Contains(":=")
                        ? CommandType.Assignment
                        : (tLine.Contains("&")
                            ? CommandType.And
                            : (tLine.Contains("|")
                                ? CommandType.Or
                                : (tLine.Contains("!=")
                                    ? CommandType.UnEqual
                                    : (tLine.Contains("++")
                                        ? CommandType.Increment
                                        : (tLine.Contains("--")
                                            ? CommandType.Decrement
                                            : (tLine.Contains("<<")
                                                ? CommandType.ShiftLeft
                                                : (tLine.Contains(">>")
                                                    ? CommandType.ShiftRight
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
                                                                        : (tLine.Contains(">"))
                                                                            ? CommandType.Bigger
                                                                            : (tLine.Contains("<"))
                                                                                ? CommandType.Smaller
                                                                                : (tLine.Contains("!"))
                                                                                    ? CommandType.Not
                                                                                    : (tLine.Contains("="))
                                                                                        ? CommandType.Equal
                                                                                        : CommandType
                                                                                            .VariableConstantMethodCallOrNothing))))))))))));
            }
        }

        private static Variable GetVariable(string variableName)
        {
            return VariableList.FirstOrDefault(
                variable => string.Equals(variable.Name, variableName, StringComparison.CurrentCultureIgnoreCase));
        }

        private static Method GetMethod(string methodName)
            =>
            MehtodList.FirstOrDefault(
                method => string.Equals(method.Name, methodName, StringComparison.CurrentCultureIgnoreCase));

        private static Tuple<VariableCall, VariableCall> GetParametersWithDivider(char divider, string line)
        {
            var ss = line.Split(divider).Select(s => s.Trim()).ToArray();
            if(ss.Length!=2)
                throw new ParameterException();
            var var1 = GetVariableConstantMethodCallOrNothing(ss[0]);
            var var2 = GetVariableConstantMethodCallOrNothing(ss[1]);
            if (var1 == null || var2 == null)
                throw new InvalidCommandException(line);
            var bitVariable = var1 as BitVariableCall;
            if (var1 is VariableCall)
                return bitVariable != null
                    ? new Tuple<VariableCall, VariableCall>((BitVariableCall)var1,
                        (BitVariableCall)var2)
                    : new Tuple<VariableCall, VariableCall>((ByteVariableCall)var1,
                        (ByteVariableCall)var2);
            throw new ParameterException();
        }

        private static Tuple<VariableCall, VariableCall> GetParametersWithDivider(string divider, string line)
        {
            var ss = Split(line, divider).Select(s => s.Trim()).ToArray();
            if (ss.Length != 2)
                throw new ParameterException();
            var var1 = GetVariableConstantMethodCallOrNothing(ss[0]);
            var var2 = GetVariableConstantMethodCallOrNothing(ss[1]);
            if (var1 == null || var2 == null)
                throw new InvalidCommandException(line);
            var bitVariable = var1 as BitVariableCall;
            if (var1 is VariableCall)
                return bitVariable != null
                    ? new Tuple<VariableCall, VariableCall>((BitVariableCall)var1,
                        (BitVariableCall)var2)
                    : new Tuple<VariableCall, VariableCall>((ByteVariableCall)var1,
                        (ByteVariableCall)var2);
            throw new ParameterException();
        }

        private static VariableCall GetParameter(char divider, string line)
        {
            var ss = line.Trim(divider).Trim();
            if (ss.Contains(' '))
                throw new ParameterException();
            var var1 = GetVariableConstantMethodCallOrNothing(ss);
            if (var1 == null)
                throw new InvalidCommandException(line);
            var bitVariable = var1 as BitVariableCall;
            if (!(var1 is VariableCall))
                throw new ParameterException();
            if (bitVariable != null) return bitVariable;
            return (ByteVariableCall) var1;
        }

        private static VariableCall GetParameter(string divider, string line)
        {
            var ss = Trim(divider, line).Trim();
            if (ss.Contains(' '))
                throw new ParameterException();
            var var1 = GetVariableConstantMethodCallOrNothing(ss);
            if (var1 == null)
                throw new InvalidCommandException(line);
            var bitVariable = var1 as BitVariableCall;
            if (!(var1 is VariableCall))
                throw new ParameterException();
            if (bitVariable != null) return bitVariable;
            return (ByteVariableCall)var1;
        }

        private static string Trim(string trimmer, string tstring)
        {
            while (tstring.EndsWith(trimmer))
                tstring = tstring.Substring(0, tstring.Length - trimmer.Length);
            while (tstring.StartsWith(trimmer))
                tstring = tstring.Substring(trimmer.Length, tstring.Length - trimmer.Length);
            return tstring;
        }

        private static string GetVariableDefinitionName(string line)
        {
            if (line.Split(' ').Length != 2)
                throw new ParameterException();
            return line.Split(' ')[1];
        }

        private static Condition GetCondition(string line)
        {
            var sb = new StringBuilder();
            if(!(line.Contains('[') && line.Contains(']')))
                throw new InvalidSyntaxException();
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
            var splitted = Split(tLine, ":=").ToArray();
            splitted = splitted.Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
            if(splitted.Length != 2)
                throw new ParameterException();
            return new Tuple<Variable, ReturningCommand>(GetVariable(splitted[0]), GetReturningCommand(splitted[1]));
        }

        private static IEnumerable<string> Split(string toSplit, string splitter)
        {
            var fin = new List<string>();
            var sb = new StringBuilder();
            var ts = toSplit.ToCharArray();
            var s = splitter.ToCharArray();

            for (var i = 0; i <= ts.Length - (s.Length - 1); i++)
            {
                if (s.Where((t, j) => t != ts[i + j]).Any())
                {
                    sb.Append(ts[i]);
                    continue;
                }
                fin.Add(sb.ToString());
                sb = new StringBuilder();
                i += splitter.Length - 1;
            }

            if (sb.Length > 0)
                fin.Add(sb.ToString());

            return fin;
        }

        private static ReturningCommand GetReturningCommand(string line)
            =>
            GetVariableConstantMethodCallOrNothing(line) as ReturningCommand ?? GetOperation(GetCommandType(line), line);
    }
}