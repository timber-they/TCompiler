#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TCompiler.Enums;
using TCompiler.Settings;
using TCompiler.Types;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes;
using TCompiler.Types.CompilingTypes.Block;
using TCompiler.Types.CompilingTypes.ReturningCommand;
using TCompiler.Types.CompilingTypes.ReturningCommand.Method;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;
using Char = TCompiler.Types.CompilingTypes.ReturningCommand.Variable.Char;

#endregion

namespace TCompiler.Compiling
{
    public static class ParseToObjects
    {
        private static List<Block> _blockList;
        private static List<Variable> _variableList;
        private static List<Method> _methodList;
        private static Method _currentMethod;
        public static int CurrentRegisterAddress = -1;
        private static int _byteCounter;
        private static IntPair _bitCounter;
        private static int _methodCounter;
        public static int Line { get; private set; }

        private static int ByteCounter
        {
            get
            {
                _byteCounter++;
                if (_byteCounter >= 0x80)
                    throw new TooManyValuesException(Line);
                return _byteCounter;
            }
        }

        private static string MethodCounter
        {
            get
            {
                _methodCounter++;
                return $"M{_methodCounter}";
            }
        }

        private static IntPair BitCounter
        {
            get
            {
                IncreaseBitCounter();
                return _bitCounter;
            }
        }

        public static string CurrentRegister
        {
            get
            {
                CurrentRegisterAddress++;
                if (CurrentRegisterAddress > 9)
                    throw new TooManyRegistersException(Line);
                return $"R{CurrentRegisterAddress}";
            }
        }

        private static void IncreaseBitCounter()
        {
            if (_bitCounter.Item2 < 7)
                _bitCounter.Item2++;
            else
            {
                _bitCounter.Item1++;
                _bitCounter.Item2 = 0;
                if (_bitCounter.Item1 >= 0x30)
                    throw new TooManyBoolsException(Line);
            }
        }

        private static void DecreaseBitCounter()
        {
            if (_bitCounter.Item2 > 0)
                _bitCounter.Item2--;
            else
            {
                _bitCounter.Item1--;
                _bitCounter.Item2 = 7;
            }
        }

        public static IEnumerable<Command> ParseTCodeToCommands(string tCode)
        {
            _byteCounter = 0x30;
            _bitCounter = new IntPair(0x20, 0x2F);
            ParseToAssembler.LabelCount = -1;
            tCode = tCode.ToLower();
            var splitted = tCode.Split('\n').Select(s => string.Join("", s.TakeWhile(c => c != ';')).Trim()).ToList();
            var fin = new List<Command>();
            Line = 0;
            CurrentRegisterAddress = -1;
            _methodCounter = -1;
            _methodList = new List<Method>();
            _variableList = new List<Variable>(GlobalSettings.StandardVariables);
            _blockList = new List<Block>();
            _currentMethod = null;

            AddMethods(splitted);

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
                            {
                                _variableList.Remove(variable);
                                if (variable is ByteVariable)
                                    _byteCounter--;
                                else
                                    DecreaseBitCounter();
                            }

                            if (_blockList.Last() is ForTilBlock)
                                CurrentRegisterAddress--;

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
                                new Label(ParseToAssembler.Label.ToString()),
                                new Int(false, CurrentRegisterAddress.ToString(), CurrentRegister));
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
                            var m = _methodList.FirstOrDefault(method => method.GetName().Equals(tLine.Split(' ', '[')[1]));
                            fin.Add(m);
                            _currentMethod = m;
                            break;
                        }
                    case CommandType.EndMethod:
                        {
                            fin.Add(new EndMethod(_currentMethod));
                            if (_currentMethod?.Variables != null)
                                foreach (var variable in _currentMethod.Variables)
                                    _variableList.Remove(variable);
                            if (_currentMethod?.Parameters != null)
                                foreach (var parameter in _currentMethod.Parameters)
                                    _variableList.Remove(parameter);
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
                    case CommandType.BitOf:
                        {
                            var op = (BitOf) GetOperation(type, tLine);
                            op.RegisterLoop = CurrentRegister;
                            fin.Add(op);
                            CurrentRegisterAddress--;
                            break;
                        }
                    case CommandType.Assignment:
                    case CommandType.AddAssignment:
                    case CommandType.SubtractAssignment:
                    case CommandType.MultiplyAssignment:
                    case CommandType.DivideAssignment:
                    case CommandType.ModuloAssignment:
                    case CommandType.AndAssignment:
                    case CommandType.OrAssignment:
                        fin.Add(GetAssignment(tLine, type));
                        break;
                    case CommandType.Bool:
                        {
                            var b = new Bool(false, BitCounter.ToString(), GetVariableDefinitionName(tLine));
                            fin.Add(AddVariable(b, tLine));
                            break;
                        }
                    case CommandType.Char:
                        {
                            var c = new Char(false, ByteCounter.ToString(), GetVariableDefinitionName(tLine));
                            fin.Add(AddVariable(c, tLine));
                            break;
                        }
                    case CommandType.Int:
                        {
                            var i = new Int(false, ByteCounter.ToString(), GetVariableDefinitionName(tLine));
                            fin.Add(AddVariable(i, tLine));
                            break;
                        }
                    case CommandType.Cint:
                        {
                            var ci = new Cint(false, ByteCounter.ToString(), GetVariableDefinitionName(tLine));
                            fin.Add(AddVariable(ci, tLine));
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

        private static Declaration AddVariable(Variable variable, string tLine)
        {
            if (
                _variableList.Any(
                    var =>
                        var.GetName()
                            .Equals(variable.GetName(), StringComparison.CurrentCultureIgnoreCase)))
                throw new VariableExistsException(Line);
            if (!IsNameValid(variable.GetName()))
                throw new InvalidNameException(Line);
            _variableList.Add(variable);
            if (_blockList.Count > 0)
                _blockList.Last().Variables.Add(variable);
            else
                _currentMethod?.Variables.Add(variable);
            return new Declaration(GetDeclarationAssignment(tLine));
        }

        private static Assignment GetDeclarationAssignment(string line)
        {
            if (!line.Contains(":=") && !line.Contains("+=") && !line.Contains("-=") && !line.Contains("*=") &&
                !line.Contains("/=") && !line.Contains("%=") && !line.Contains("&=") && !line.Contains("|="))
                return null;
            var l = line.Substring(line.Split(' ').First().Length).Trim(' ');
            return GetAssignment(l, GetCommandType(l));
        }

        private static Assignment GetAssignment(string tLine, CommandType ct)
        {
            switch (ct)
            {
                case CommandType.Assignment:
                    {
                        var pars = GetAssignmentParameter(tLine, ":=");
                        return new Assignment(pars.Item1, pars.Item2);
                    }
                case CommandType.AddAssignment:
                    {
                        var pars = GetAssignmentParameter(tLine, "+=");
                        return new AddAssignment(pars.Item1, pars.Item2);
                    }
                case CommandType.SubtractAssignment:
                    {
                        var pars = GetAssignmentParameter(tLine, "-=");
                        return new SubtractAssignment(pars.Item1, pars.Item2);
                    }
                case CommandType.MultiplyAssignment:
                    {
                        var pars = GetAssignmentParameter(tLine, "*=");
                        return new MultiplyAssignment(pars.Item1, pars.Item2);
                    }
                case CommandType.DivideAssignment:
                    {
                        var pars = GetAssignmentParameter(tLine, "/=");
                        return new DivideAssignment(pars.Item1, pars.Item2);
                    }
                case CommandType.ModuloAssignment:
                    {
                        var pars = GetAssignmentParameter(tLine, "%=");
                        return new ModuloAssignment(pars.Item1, pars.Item2);
                    }
                case CommandType.AndAssignment:
                    {
                        var pars = GetAssignmentParameter(tLine, "&=");
                        return new AndAssignment(pars.Item1, pars.Item2);
                    }
                case CommandType.OrAssignment:
                    {
                        var pars = GetAssignmentParameter(tLine, "|=");
                        return new OrAssignment(pars.Item1, pars.Item2);
                    }
                default:
                    return null;
            }
        }

        private static void AddMethods(List<string> lines)
        {
            Line = -1;
            foreach (var tLine in lines)
            {
                Line++;
                if (GetCommandType(tLine) != CommandType.Method)
                    continue;
                var name = tLine.Split(' ', '[')[1].Split('[').First();
                if (!IsNameValid(name))
                    throw new InvalidNameException(Line);
                _methodList.Add(new Method(name, GetMethodParameters(tLine), MethodCounter));
            }
            Line = 0;
        }

        private static bool IsNameValid(string name) => (name.Length > 0) && GlobalSettings.InvalidNames.All(
                                                            s =>
                                                                !s.Equals(name,
                                                                    StringComparison.CurrentCultureIgnoreCase)) &&
                                                        char.IsLetter(name[0]);

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
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new ParameterException(Line);
                    return new BitOf((ByteVariableCall) vars.Item1, (ByteVariableCall) vars.Item2,
                        ParseToAssembler.Label, ParseToAssembler.Label, ParseToAssembler.Label);
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
            {
                var values = GetMethodParameterValues(tLine, method.Parameters);
                return new MethodCall(method, values);
            }

            var variable = GetVariable(tLine);
            if ((variable != null) && !(variable is BitOfVariable))
            {
                var byteVariable = variable as ByteVariable;
                if (byteVariable != null)
                    return new ByteVariableCall(byteVariable);
                return new BitVariableCall((BitVariable) variable);
            }

            bool b;
            if (bool.TryParse(tLine, out b))
                return new BitVariableCall(new Bool(true, null, null, b));

            uint ui; //TODO check if value should be cint
            if ((tLine.StartsWith("0x") &&
                 uint.TryParse(Trim("0x", tLine), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out ui)) ||
                uint.TryParse(tLine, NumberStyles.None, CultureInfo.CurrentCulture, out ui))
            {
                if (ui > 255)
                    throw new InvalidValueException(Line);
                return new ByteVariableCall(new Int(true, null, null, Convert.ToByte(ui)));
            }

            int i;
            if (!tLine.Contains('.') &&
                ((tLine.StartsWith("0x") && int.TryParse(Trim("0x", tLine), 0 << 9, CultureInfo.CurrentCulture, out i)) ||
                 int.TryParse(tLine, NumberStyles.Number, CultureInfo.CurrentCulture, out i)))
            {
                if ((i > 127) || (i < -128))
                    throw new InvalidValueException(Line);
                return new ByteVariableCall(new Cint(true, null, null, (byte) Convert.ToSByte(i)));
            }

            char c;
            if (tLine.StartsWith("'") && tLine.EndsWith("'") && char.TryParse(tLine.Trim('\''), out c))
                return new ByteVariableCall(new Char(true, null, null, (byte) c));

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
            switch (tLine.Split(' ', '[').FirstOrDefault())
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
                case "{":
                case "block":
                    return CommandType.Block;
                case "}":
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
                        : (tLine.Contains("+=")
                            ? CommandType.AddAssignment
                            : (tLine.Contains("-=")
                                ? CommandType.SubtractAssignment
                                : (tLine.Contains("*=")
                                    ? CommandType.MultiplyAssignment
                                    : (tLine.Contains("/=")
                                        ? CommandType.DivideAssignment
                                        : (tLine.Contains("%=")
                                            ? CommandType.ModuloAssignment
                                            : (tLine.Contains("|=")
                                                ? CommandType.OrAssignment
                                                : (tLine.Contains("&=")
                                                    ? CommandType.AndAssignment
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
                                                                                                        ? CommandType
                                                                                                            .Bigger
                                                                                                        : tLine.Contains
                                                                                                            ("<")
                                                                                                            ? CommandType
                                                                                                                .Smaller
                                                                                                            : tLine
                                                                                                                .Contains
                                                                                                                ("!")
                                                                                                                ? CommandType
                                                                                                                    .Not
                                                                                                                : tLine
                                                                                                                    .Contains
                                                                                                                    ("=")
                                                                                                                    ? CommandType
                                                                                                                        .Equal
                                                                                                                    : tLine
                                                                                                                        .Contains
                                                                                                                        (".")
                                                                                                                        ? CommandType
                                                                                                                            .BitOf
                                                                                                                        : CommandType
                                                                                                                            .VariableConstantMethodCallOrNothing)))))))))))))))))));
            }
        }

        private static Variable GetVariable(string variableName)
        {
            var var = _variableList.FirstOrDefault(
                variable =>
                    string.Equals(variable.GetName(), variableName.Split('.').First(),
                        StringComparison.CurrentCultureIgnoreCase));

            if (!variableName.Contains('.'))
                return var;

            var bit =
                GetVariableConstantMethodCallOrNothing(variableName.Split('.').LastOrDefault()) as ByteVariableCall;
            if (bit == null)
                throw new ParameterException(Line);
            return new BitOfVariable(var?.Address, bit.Variable, ParseToAssembler.Label, ParseToAssembler.Label,
                ParseToAssembler.Label, ParseToAssembler.Label);
        }

        private static Method GetMethod(string methodName)
            =>
            _methodList.FirstOrDefault(
                method =>
                    string.Equals(method.GetName(), methodName.Split(' ', ']').FirstOrDefault(),
                        StringComparison.CurrentCultureIgnoreCase));

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
            if (bitVariable != null)
                return bitVariable;
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
            if (bitVariable != null)
                return bitVariable;
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
            if (line.Split(' ').Length < 2)
                throw new ParameterException(Line);
            return line.Split(' ')[1];
        }

        private static Condition GetCondition(string line)
            => new Condition(GetReturningCommand(GetStringBetween('[', ']', line)));

        private static string GetStringBetween(char start, char end, string line)
        {
            var sb = new StringBuilder();
            if (!(line.Contains(start) && line.Contains(end)))
                throw new InvalidSyntaxException(Line);
            foreach (var s in line.Split(start)[1])
            {
                if (s == end)
                    break;
                sb.Append(s);
            }
            return sb.ToString();
        }

        private static List<Variable> GetMethodParameters(string line)
        {
            var content = GetStringBetween('[', ']', line);
            var parameterAsStrings =
                content.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim(' '));
            var fin = new List<Variable>();
            foreach (var parameterAsString in parameterAsStrings)
            {
                var type = GetCommandType(parameterAsString);
                switch (type)
                {
                    case CommandType.Int:
                        {
                            var i = new Int(false, ByteCounter.ToString(), GetVariableDefinitionName(parameterAsString));
                            if (
                                _variableList.Any(
                                    variable =>
                                        variable.GetName()
                                            .Equals(i.GetName(), StringComparison.CurrentCultureIgnoreCase)))
                                throw new VariableExistsException(Line);
                            if (!IsNameValid(i.GetName()))
                                throw new InvalidNameException(Line);
                            fin.Add(i);
                            _variableList.Add(i);
                            break;
                        }
                    case CommandType.Cint:
                        {
                            var ci = new Cint(false, ByteCounter.ToString(), GetVariableDefinitionName(parameterAsString));
                            if (
                                _variableList.Any(
                                    variable =>
                                        variable.GetName()
                                            .Equals(ci.GetName(), StringComparison.CurrentCultureIgnoreCase)))
                                throw new VariableExistsException(Line);
                            if (!IsNameValid(ci.GetName()))
                                throw new InvalidNameException(Line);
                            fin.Add(ci);
                            _variableList.Add(ci);
                            break;
                        }
                    case CommandType.Char:
                        {
                            var c = new Char(false, ByteCounter.ToString(), GetVariableDefinitionName(parameterAsString));
                            if (
                                _variableList.Any(
                                    variable =>
                                        variable.GetName()
                                            .Equals(c.GetName(), StringComparison.CurrentCultureIgnoreCase)))
                                throw new VariableExistsException(Line);
                            if (!IsNameValid(c.GetName()))
                                throw new InvalidNameException(Line);
                            fin.Add(c);
                            _variableList.Add(c);
                            break;
                        }
                    case CommandType.Bool:
                        {
                            var b = new Bool(false, BitCounter.ToString(), GetVariableDefinitionName(parameterAsString));
                            if (
                                _variableList.Any(
                                    variable =>
                                        variable.GetName()
                                            .Equals(b.GetName(), StringComparison.CurrentCultureIgnoreCase)))
                                throw new VariableExistsException(Line);
                            if (!IsNameValid(b.GetName()))
                                throw new InvalidNameException(Line);
                            fin.Add(b);
                            _variableList.Add(b);
                            break;
                        }
                    default:
                        throw new ParameterException(Line, "Invalid Parameter type!");
                }
            }
            return fin;
        }

        private static List<VariableCall> GetMethodParameterValues(string line, IReadOnlyList<Variable> parameters)
        {
            var fin = new List<VariableCall>();
            var rawValues =
                GetStringBetween('[', ']', line)
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();
            if (rawValues.Count != parameters.Count)
                throw new ParameterException(Line, "Wrong parameter count!");
            for (var index = 0; index < rawValues.Count; index++)
            {
                var value = rawValues[index];
                var v = GetVariableConstantMethodCallOrNothing(value) as VariableCall;
                var parameter = parameters[index];
                if ((v == null) || (parameter is ByteVariable && v is BitVariableCall) ||
                    (parameter is BitVariable && v is ByteVariableCall))
                    throw new ParameterException(Line, "Wrong parameter type");
                fin.Add(v);
            }
            return fin;
        }

        private static Tuple<Variable, ReturningCommand> GetAssignmentParameter(string tLine, string splitter)
        {
            var splitted = Split(tLine, splitter).ToArray();
            splitted = splitted.Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
            var variable = GetVariable(splitted[0]);
            if ((splitted.Length != 2) || (variable == null))
                throw new ParameterException(Line);
            return new Tuple<Variable, ReturningCommand>(variable, GetReturningCommand(splitted[1]));
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