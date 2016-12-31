using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TCompiler.Enums;
using TCompiler.Settings;
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

namespace TCompiler.Compiling
{
    public static class ParseToObjects
    {
        private static List<Block> _blockList;
        private static List<Variable> _variableList;
        private static List<Method> _methodList;
        private static Method _currentMethod;
        public static int CurrentRegister1 = -1;
        public static int Line { get; private set; }

        public static string CurrentRegister
        {
            get
            {
                CurrentRegister1++;
                if (CurrentRegister1 > 9)
                    throw new TooManyRegistersException(Line);
                return $"R{CurrentRegister1}";
            }
        }

        public static IEnumerable<Command> ParseTCodeToCommands(string tCode)
        {
            ParseToAssembler.LabelCount = -1;
            tCode = tCode.ToLower();
            var splitted = tCode.Split('\n').Select(s => s.Trim()).ToArray();
            var fin = new List<Command>();
            Line = 0;
            CurrentRegister1 = -1;
            _methodList = new List<Method>();
            _variableList = new List<Variable>(GlobalSettings.StandardVariables);
            _blockList = new List<Block>();
            _currentMethod = null;

            foreach (var tLine in splitted)
            {
                var type = GetCommandType(tLine);
                switch (type)
                {
                    case CommandType.VariableConstantMethodCallOrNothing:
                    {
                        // ReSharper disable once IdentifierTypo
                        var vcmn = GetVariableConstantMethodCallOrNothing(tLine);
                        if (vcmn != null)
                            fin.Add(vcmn);
                        else
                            throw new InvalidCommandException(Line, tLine);
                        break;
                    }
                    case CommandType.Block:
                    {
                        var b = new Block(null);
                        _blockList.Add(b);
                        fin.Add(b);
                        break;
                    }
                    case CommandType.EndForTil:
                    case CommandType.EndWhile:
                    case CommandType.EndIf:
                    case CommandType.EndBlock:
                    {
                        var l = new Label(ParseToAssembler.Label.ToString());
                        fin.Add(new EndBlock(_blockList.Last()));
                        _blockList.Last().EndLabel = l;
                        foreach (var variable in _blockList.Last().Variables)
                            _variableList.Remove(variable);

                        if (_blockList.Last() is ForTilBlock)
                            CurrentRegister1--;

                        _blockList.RemoveRange(_blockList.Count - 1, 1);
                        break;
                    }
                    case CommandType.IfBlock:
                    {
                        var b = new IfBlock(null, GetCondition(tLine));
                        _blockList.Add(b);
                        fin.Add(b);
                        break;
                    }
                    case CommandType.WhileBlock:
                    {
                        var b = new WhileBlock(null, GetCondition(tLine), new Label(ParseToAssembler.Label.ToString()));
                        _blockList.Add(b);
                        fin.Add(b);
                        break;
                    }
                    case CommandType.ForTilBlock:
                    {
                        var b = new ForTilBlock(null, GetParameterForTil(tLine),
                            new Label(ParseToAssembler.Label.ToString()), new Int(false, CurrentRegister));
                        _blockList.Add(b);
                        fin.Add(b);
                        break;
                    }
                    case CommandType.Break:
                    {
                        fin.Add(new Break(_blockList.Last()));
                        break;
                    }
                    case CommandType.Method:
                    {
                        var m = new Method(tLine.Split(' ')[1]);
                        _methodList.Add(m);
                        fin.Add(m);
                        _currentMethod = m;
                        break;
                    }
                    case CommandType.EndMethod:
                    {
                        fin.Add(new EndMethod(_currentMethod));
                        if (_currentMethod?.Variables != null)
                            foreach (var variable in _currentMethod?.Variables)
                                _variableList.Remove(variable);
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
                    case CommandType.BitOf:
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
                        if (
                            _variableList.Any(
                                variable => variable.Name.Equals(b.Name, StringComparison.CurrentCultureIgnoreCase)))
                            throw new VariableExistsException(Line);
                        if (!IsNameValid(b.Name))
                            throw new InvalidNameException(Line);
                        fin.Add(b);
                        _variableList.Add(b);
                        if (_blockList.Count > 0)
                            _blockList.Last().Variables.Add(b);
                        else
                            _currentMethod?.Variables.Add(b);

                        break;
                    }
                    case CommandType.Char:
                    {
                        var c = new Char(false, GetVariableDefinitionName(tLine));
                        if (
                            _variableList.Any(
                                variable => variable.Name.Equals(c.Name, StringComparison.CurrentCultureIgnoreCase)))
                            throw new VariableExistsException(Line);
                        if (!IsNameValid(c.Name))
                            throw new InvalidNameException(Line);
                        fin.Add(c);
                        _variableList.Add(c);
                        if (_blockList.Count > 0)
                            _blockList.Last().Variables.Add(c);
                        else
                            _currentMethod?.Variables.Add(c);
                        break;
                    }
                    case CommandType.Int:
                    {
                        var i = new Int(false, GetVariableDefinitionName(tLine));
                        if (
                            _variableList.Any(
                                variable => variable.Name.Equals(i.Name, StringComparison.CurrentCultureIgnoreCase)))
                            throw new VariableExistsException(Line);
                        if (!IsNameValid(i.Name))
                            throw new InvalidNameException(Line);
                        fin.Add(i);
                        _variableList.Add(i);
                        if (_blockList.Count > 0)
                            _blockList.Last().Variables.Add(i);
                        else
                            _currentMethod?.Variables.Add(i);
                        break;
                    }
                    case CommandType.Cint:
                    {
                        var ci = new Cint(false, GetVariableDefinitionName(tLine));
                        if (
                            _variableList.Any(
                                variable => variable.Name.Equals(ci.Name, StringComparison.CurrentCultureIgnoreCase)))
                            throw new VariableExistsException(Line);
                        if (!IsNameValid(ci.Name))
                            throw new InvalidNameException(Line);
                        fin.Add(ci);
                        _variableList.Add(ci);
                        if (_blockList.Count > 0)
                            _blockList.Last().Variables.Add(ci);
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

                Line++;
            }

            return fin;
        }

        private static bool IsNameValid(string name) => !name.StartsWith("l") &&
                                                        GlobalSettings.InvalidNames.All(
                                                            s =>
                                                                !s.Equals(name,
                                                                    StringComparison.CurrentCultureIgnoreCase));

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
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new ParameterException(Line);
                    return new Add((ByteVariableCall) vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Subtract:
                    vars = GetParametersWithDivider('-', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new ParameterException(Line);
                    return new Subtract((ByteVariableCall) vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Multiply:
                    vars = GetParametersWithDivider('*', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new ParameterException(Line);
                    return new Multiply((ByteVariableCall) vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Divide:
                    vars = GetParametersWithDivider('/', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new ParameterException(Line);
                    return new Divide((ByteVariableCall) vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Modulo:
                    vars = GetParametersWithDivider('%', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new ParameterException(Line);
                    return new Modulo((ByteVariableCall) vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Bigger:
                    vars = GetParametersWithDivider('>', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new ParameterException(Line);
                    return new Bigger((ByteVariableCall) vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Smaller:
                    vars = GetParametersWithDivider('<', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new ParameterException(Line);
                    return new Smaller((ByteVariableCall) vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Equal:
                    vars = GetParametersWithDivider('=', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new ParameterException(Line);
                    return new Equal((ByteVariableCall) vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.UnEqual:
                    vars = GetParametersWithDivider("!=", line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new ParameterException(Line);
                    return new UnEqual((ByteVariableCall) vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Increment:
                    try
                    {
                        return new Increment((ByteVariableCall) GetParameter("++", line));
                    }
                    catch (InvalidCastException)
                    {
                        throw new ParameterException(Line);
                    }
                case CommandType.Decrement:
                    try
                    {
                        return new Decrement((ByteVariableCall) GetParameter("--", line));
                    }
                    catch (InvalidCastException)
                    {
                        throw new ParameterException(Line);
                    }
                case CommandType.ShiftLeft:
                    vars = GetParametersWithDivider("<<", line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new ParameterException(Line);
                    return new ShiftLeft((ByteVariableCall) vars.Item1, (ByteVariableCall) vars.Item2, CurrentRegister,
                        ParseToAssembler.Label);
                case CommandType.ShiftRight:
                    vars = GetParametersWithDivider("<<", line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new ParameterException(Line);
                    return new ShiftRight((ByteVariableCall) vars.Item1, (ByteVariableCall) vars.Item2, CurrentRegister,
                        ParseToAssembler.Label);
                case CommandType.BitOf:
                    vars = GetParametersWithDivider('.', line);
                    if ((vars.Item2.GetType() != typeof(ByteVariableCall)) ||
                        !((ByteVariableCall) vars.Item2).Variable.IsConstant)
                        throw new ParameterException(Line);
                    return new BitOf((ByteVariableCall) vars.Item1, (ByteVariableCall) vars.Item2,
                        ParseToAssembler.Label, ParseToAssembler.Label);
                default:
                    return null;
            }
        }

        private static Command GetVariableConstantMethodCallOrNothing(string tLine)
        {
            if (string.IsNullOrEmpty(tLine))
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
                return new BitVariableCall((BitVariable) variable);
            }

            bool b;
            if (bool.TryParse(tLine, out b))
                return new BitVariableCall(new Bool(true, null, b));

            uint ui;                                                                                                    //TODO check if value should be cint
            if ((tLine.StartsWith("0x") &&
                 uint.TryParse(Trim("0x", tLine), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out ui)) ||
                uint.TryParse(tLine, NumberStyles.None, CultureInfo.CurrentCulture, out ui))
            {
                if (ui > 255)
                    throw new InvalidValueException(Line);
                return new ByteVariableCall(new Int(true, null, Convert.ToByte(ui)));
            }

            int i;
            if (!tLine.Contains('.') &&
                ((tLine.StartsWith("0x") && int.TryParse(Trim("0x", tLine), 0 << 9, CultureInfo.CurrentCulture, out i)) ||
                 int.TryParse(tLine, NumberStyles.Number, CultureInfo.CurrentCulture, out i)))
            {
                if ((i > 127) || (i < -128))
                    throw new InvalidValueException(Line);
                return new ByteVariableCall(new Cint(true, null, (byte) Convert.ToSByte(i)));
            }

            char c;
            if (tLine.StartsWith("'") && tLine.EndsWith("'") && char.TryParse(tLine.Trim('\''), out c))
                return new ByteVariableCall(new Char(true, null, (byte) c));

            return null;
        }

        private static ByteVariableCall GetParameterForTil(string line)
        {
            var p = GetVariableConstantMethodCallOrNothing(line.Trim().Split(' ')[1]) as ByteVariableCall;
            if (p == null)
                throw new InvalidCommandException(Line, line);
            return p;
        }

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
                                                                        : tLine.Contains(">")
                                                                            ? CommandType.Bigger
                                                                            : tLine.Contains("<")
                                                                                ? CommandType.Smaller
                                                                                : tLine.Contains("!")
                                                                                    ? CommandType.Not
                                                                                    : tLine.Contains("=")
                                                                                        ? CommandType.Not
                                                                                        : tLine.Contains(".")
                                                                                            ? CommandType.BitOf
                                                                                            : CommandType
                                                                                                .VariableConstantMethodCallOrNothing))))))))))));
            }
        }

        private static Variable GetVariable(string variableName)
        {
            return _variableList.FirstOrDefault(
                variable => string.Equals(variable.Name, variableName, StringComparison.CurrentCultureIgnoreCase));
        }

        private static Method GetMethod(string methodName)
            =>
            _methodList.FirstOrDefault(
                method => string.Equals(method.Name, methodName, StringComparison.CurrentCultureIgnoreCase));

        private static Tuple<VariableCall, VariableCall> GetParametersWithDivider(char divider, string line)
        {
            var ss = line.Split(divider).Select(s => s.Trim()).ToArray();
            if (ss.Length != 2)
                throw new ParameterException(Line);
            var var1 = GetVariableConstantMethodCallOrNothing(ss[0]);
            var var2 = GetVariableConstantMethodCallOrNothing(ss[1]);
            if ((var1 == null) || (var2 == null))
                throw new InvalidCommandException(Line, line);
            var bitVariable = var1 as BitVariableCall;
            if (var1 is VariableCall && (var1.GetType() == var2.GetType()))
                return bitVariable != null
                    ? new Tuple<VariableCall, VariableCall>((BitVariableCall) var1,
                        (BitVariableCall) var2)
                    : new Tuple<VariableCall, VariableCall>((ByteVariableCall) var1,
                        (ByteVariableCall) var2);
            throw new ParameterException(Line);
        }

        private static Tuple<VariableCall, VariableCall> GetParametersWithDivider(string divider, string line)
        {
            var ss = Split(line, divider).Select(s => s.Trim()).ToArray();
            if (ss.Length != 2)
                throw new ParameterException(Line);
            var var1 = GetVariableConstantMethodCallOrNothing(ss[0]);
            var var2 = GetVariableConstantMethodCallOrNothing(ss[1]);
            if ((var1 == null) || (var2 == null))
                throw new InvalidCommandException(Line, line);
            var bitVariable = var1 as BitVariableCall;
            if (var1 is VariableCall)
                return bitVariable != null
                    ? new Tuple<VariableCall, VariableCall>((BitVariableCall) var1,
                        (BitVariableCall) var2)
                    : new Tuple<VariableCall, VariableCall>((ByteVariableCall) var1,
                        (ByteVariableCall) var2);
            throw new ParameterException(Line);
        }

        private static VariableCall GetParameter(char divider, string line)
        {
            var ss = line.Trim(divider).Trim();
            if (ss.Contains(' '))
                throw new ParameterException(Line);
            var var1 = GetVariableConstantMethodCallOrNothing(ss);
            if (var1 == null)
                throw new InvalidCommandException(Line, line);
            var bitVariable = var1 as BitVariableCall;
            if (!(var1 is VariableCall))
                throw new ParameterException(Line);
            if (bitVariable != null) return bitVariable;
            return (ByteVariableCall) var1;
        }

        private static VariableCall GetParameter(string divider, string line)
        {
            var ss = Trim(divider, line).Trim();
            if (ss.Contains(' '))
                throw new ParameterException(Line);
            var var1 = GetVariableConstantMethodCallOrNothing(ss);
            if (var1 == null)
                throw new InvalidCommandException(Line, line);
            var bitVariable = var1 as BitVariableCall;
            if (!(var1 is VariableCall))
                throw new ParameterException(Line);
            if (bitVariable != null) return bitVariable;
            return (ByteVariableCall) var1;
        }

        private static string Trim(string trimmer, string tString)
        {
            while (tString.EndsWith(trimmer))
                tString = tString.Substring(0, tString.Length - trimmer.Length);
            while (tString.StartsWith(trimmer))
                tString = tString.Substring(trimmer.Length, tString.Length - trimmer.Length);
            return tString;
        }

        private static string GetVariableDefinitionName(string line)
        {
            if (line.Split(' ').Length != 2)
                throw new ParameterException(Line);
            return line.Split(' ')[1];
        }

        private static Condition GetCondition(string line)
        {
            var sb = new StringBuilder();
            if (!(line.Contains('[') && line.Contains(']')))
                throw new InvalidSyntaxException(Line);
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
            if (splitted.Length != 2)
                throw new ParameterException(Line);
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