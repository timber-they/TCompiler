#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCompiler.Enums;
using TCompiler.General;
using TCompiler.Settings;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes;
using TCompiler.Types.CompilingTypes.Block;
using TCompiler.Types.CompilingTypes.ReturningCommand;
using TCompiler.Types.CompilingTypes.ReturningCommand.Method;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;
using TCompiler.Types.CompilingTypes.TemporaryOperation.TemporaryParsedStringOperation;
using Char = TCompiler.Types.CompilingTypes.ReturningCommand.Variable.Char;

#endregion

namespace TCompiler.Compiling
{
    /// <summary>
    ///     Parses the given TCode to objects
    /// </summary>
    public static class ParseToObjects
    {
        /// <summary>
        ///     Parses the given TCode to CommandObjects
        /// </summary>
        /// <param name="tCode">The TCode that shall get parsed</param>
        /// <exception cref="ParameterException"></exception>
        /// <exception cref="InvalidCommandException"></exception>
        /// <exception cref="ElseWithoutIfException"></exception>
        /// <exception cref="InvalidNameException"></exception>
        /// <returns>A list of the parsed CommandObjects</returns>
        /// <exception cref="ArgumentOutOfRangeException">This shouldn't get thrown</exception>
        public static IEnumerable<Command> ParseTCodeToCommands(string tCode)
        {
            InitializeVariables();

            tCode = GetTCodeWithInsertedSpaces(tCode.ToLower());

            var splitted = tCode.Split('\n').Select(s => string.Join("", s.TakeWhile(c => c != ';')).Trim()).ToList();
            var fin = new List<Command>();

            AddMethods(splitted);

            foreach (var tLine in splitted)
            {
                var type = GetCommandType(tLine);
                switch (type)
                {
                    case CommandType.VariableConstantMethodCallOperationOrNothing:
                        {
                            var item =
                                new TemporaryParsedStringOperation(tLine)?.GeTemporaryReturning()?.Item2?.GetReturningCommand();
                            if (item != null)
                                fin.Add(item);
                            break;
                        }
                    case CommandType.Block:
                        {
                            var b = new Block(null);
                            fin.Add(GeneralBlockActions(b));
                            break;
                        }
                    case CommandType.EndForTil:
                    case CommandType.EndWhile:
                    case CommandType.EndIf:
                    case CommandType.EndBlock:
                    case CommandType.ElseBlock:
                        {
                            var l = new Label(GlobalProperties.Label);
                            if (type != CommandType.ElseBlock)
                                fin.Add(new EndBlock(_blockList.Last()));
                            _blockList.Last().EndLabel = l;

                            RemoveAtEnd(_blockList.Last().Variables, _blockList.Last() is ForTilBlock);
                            if (type == CommandType.ElseBlock)
                            {
                                var ib = _blockList.LastOrDefault() as IfBlock;
                                if (ib == null)
                                    throw new ElseWithoutIfException(GlobalProperties.LineIndex);
                                var eb = new ElseBlock(ib.EndLabel, GlobalProperties.Label);
                                ib.Else = eb;
                                _blockList.RemoveRange(_blockList.Count - 1, 1);
                                fin.Add(GeneralBlockActions(eb));
                            }
                            else
                                _blockList.RemoveRange(_blockList.Count - 1, 1);
                            break;
                        }
                    case CommandType.IfBlock:
                        {
                            var b = new IfBlock(null, GetCondition(tLine), null);
                            fin.Add(GeneralBlockActions(b));
                            break;
                        }
                    case CommandType.WhileBlock:
                        {
                            var b = new WhileBlock(null, GetCondition(tLine), new Label(GlobalProperties.Label));
                            fin.Add(GeneralBlockActions(b));
                            break;
                        }
                    case CommandType.ForTilBlock:
                        {
                            var pars = GetParameterForTil(tLine);
                            var b = new ForTilBlock(null, pars.Item1,
                                new Label(GlobalProperties.Label),
                                pars.Item2);
                            _variableList.Add(pars.Item2);
                            b.Variables.Add(pars.Item2);
                            fin.Add(GeneralBlockActions(b));
                            break;
                        }
                    case CommandType.Break:
                        {
                            fin.Add(new Break(_blockList.Last()));
                            break;
                        }
                    case CommandType.Method:
                        {
                            var m = MethodList.FirstOrDefault(method => method.Name.Equals(tLine.Split(new[] { ' ', '[' }, StringSplitOptions.RemoveEmptyEntries)[1]));
                            if (m != null)
                            {
                                _variableList.AddRange(m.Parameters);
                                fin.Add(m);
                                _currentMethod = m;
                            }
                            else
                                throw new InvalidNameException(GlobalProperties.LineIndex, tLine.Split(new[] { ' ', '[' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                            break;
                        }
                    case CommandType.InterruptServiceRoutine:
                        {
                            var typeName = GetType_NameOfInterrupt(tLine);
                            if (_usedInterrupts.Contains(typeName.Item1))
                                throw new InterruptAlreadyUsedException(GlobalProperties.LineIndex, typeName.Item1);

                            var s = tLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            var external = (typeName.Item1 == InterruptType.ExternalInterrupt0) ||
                                           (typeName.Item1 == InterruptType.ExternalInterrupt1);
                            if ((s.Length > 1) && external)
                                throw new ParameterException(GlobalProperties.LineIndex, "This operation doesn't have any parameters!");
                            if ((s.Length != 2) && !external)
                                throw new ParameterException(GlobalProperties.LineIndex, tLine.Length.ToString(),
                                    "The parameter count wasn't valid. The count was {0}");
                            var c = 0;
                            if (!external &&
                                (!int.TryParse(tLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1], out c) ||
                                 (c > 65536) || (c <= 0)))
                                throw new ParameterException(GlobalProperties.LineIndex, s[1]);
                            c = 65536 - c;
                            var low = (byte) (c % 256);
                            var high = (byte) (c / 256);
                            fin.Add(
                                new InterruptServiceRoutine(
                                    new Label(typeName.Item2), typeName.Item1, new Tuple<byte, byte>(low, high)));
                            _usedInterrupts.Add(typeName.Item1);
                            break;
                        }
                    case CommandType.EndMethod:
                        {
                            fin.Add(new EndMethod());
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
                            fin.Add(
                                new Return(tLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length > 1
                                    ? GetReturningCommand(tLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1])
                                    : null));
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
                            var b = new Bool(CurrentBitAddress, GetVariableDefinitionName(tLine), false);
                            fin.Add(GetDeclarationToVariable(b, tLine));
                            break;
                        }
                    case CommandType.Char:
                        {
                            var c = new Char(CurrentByteAddress, GetVariableDefinitionName(tLine), false);
                            fin.Add(GetDeclarationToVariable(c, tLine));
                            break;
                        }
                    case CommandType.Int:
                        {
                            var i = new Int(CurrentByteAddress, GetVariableDefinitionName(tLine), false);
                            fin.Add(GetDeclarationToVariable(i, tLine));
                            break;
                        }
                    case CommandType.Cint:
                        {
                            var ci = new Cint(CurrentByteAddress, GetVariableDefinitionName(tLine), false);
                            fin.Add(GetDeclarationToVariable(ci, tLine));
                            break;
                        }
                    case CommandType.Collection:
                        {
                            var s = tLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).First().Split('#');
                            if (s.Length != 2)
                                throw new ParameterException(GlobalProperties.LineIndex, s.Length > 2 ? s[2] : s.LastOrDefault());
                            int count;
                            if (!int.TryParse(s[1], out count))
                                throw new ParameterException(GlobalProperties.LineIndex, s[1]);
                            var c = new Collection(CurrentByteAddress, GetVariableDefinitionName(tLine), count);
                            // ReSharper disable once NotAccessedVariable
                            Address foo;
                            for (var i = 0; i < count - 1; i++)
                                // ReSharper disable once RedundantAssignment
                                foo = CurrentByteAddress;
                            fin.Add(GetDeclarationToVariable(c, tLine));
                            break;
                        }
                    case CommandType.Sleep:
                        {
                            int time;
                            var s = tLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if ((s.Length != 2) || !int.TryParse(s[1], out time))
                                throw new ParameterException(GlobalProperties.LineIndex, "Wrong or missing constant sleep time!");
                            var sleep = new Sleep(time);
                            fin.Add(sleep);
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                GlobalProperties.LineIndex++;
            }

            return fin;
        }

        /// <summary>
        /// Initializes allt the variables used here
        /// </summary>
        private static void InitializeVariables()
        {
            _usedInterrupts = new List<InterruptType>();
            _byteCounter = new Address(0x30, false);
            _bitCounter = new Address(0x20, false, 0x2F);
            GlobalProperties.LabelCount = -1;
            GlobalProperties.LineIndex = 0;
            GlobalProperties.CurrentRegisterAddress = -1;
            _methodCounter = -1;
            MethodList = new List<Method>();
            _variableList = new List<Variable>(GlobalProperties.StandardVariables);
            _blockList = new List<Block>();
            _currentMethod = null;
        }

        /// <summary>
        /// Evaluates the type and the name of the interrupt
        /// </summary>
        /// <param name="tLine">The line in which the interrupt is</param>
        /// <returns>The type and the name as a tuple</returns>
        private static Tuple<InterruptType, string> GetType_NameOfInterrupt(string tLine)
        {
            InterruptType t;
            string name;
            var relevantString = tLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
            switch (relevantString)
            {
                case "isrexternal0":
                    t = InterruptType.ExternalInterrupt0;
                    name = GlobalProperties.ExternalInterrupt0ExecutionName;
                    break;
                case "isrexternal1":
                    t = InterruptType.ExternalInterrupt1;
                    name = GlobalProperties.ExternalInterrupt1ExecutionName;
                    break;
                case "isrtimer0":
                    t = InterruptType.TimerInterrupt0;
                    name = GlobalProperties.TimerCounterInterrupt0ExecutionName;
                    break;
                case "isrcounter0":
                    t = InterruptType.CounterInterrupt0;
                    name = GlobalProperties.TimerCounterInterrupt0ExecutionName;
                    break;
                case "isrtimer1":
                    t = InterruptType.TimerInterrupt1;
                    name = GlobalProperties.TimerCounterInterrupt1ExecutionName;
                    break;
                default:
                    t = InterruptType.CounterInterrupt1;
                    name = GlobalProperties.TimerCounterInterrupt1ExecutionName;
                    break;
            }
            return new Tuple<InterruptType, string>(t, name);
        }

        /// <summary>
        /// Removes all the stuff that has to get removed at every end of a block
        /// </summary>
        private static void RemoveAtEnd(IEnumerable<Variable> variables, bool isForTilBlock)
        {
            foreach (var variable in variables)
            {
                _variableList.Remove(variable);
                if (variable is ByteVariable)
                    _byteCounter = _byteCounter.PreviousAddress;
                else if (variable is BitVariable)
                    _bitCounter = _bitCounter.PreviousAddress;
                else
                    for (var i = 0; i < ((Collection) variable).RangeCount; i++)
                        _byteCounter = _bitCounter.PreviousAddress;
            }

            if (isForTilBlock)
                GlobalProperties.CurrentRegisterAddress--;
        }

        /// <summary>
        /// Makes general block actions and evaluates the stuff to add to the final command list
        /// </summary>
        /// <param name="blockType">The type of the ending block</param>
        /// <returns>The stuff to add to the final command list</returns>
        private static Command GeneralBlockActions(Block blockType)
        {
            _blockList.Add(blockType);
            return blockType;
        }

        /// <summary>
        ///     Adds all methods from the code to the _methodList
        /// </summary>
        /// <param name="lines">All lines of the code</param>
        /// <exception cref="InvalidNameException">Gets thrown when the method name is invalid</exception>
        private static void AddMethods(IEnumerable<string> lines)
        {
            GlobalProperties.LineIndex = -1;
            foreach (var tLine in lines)
            {
                GlobalProperties.LineIndex++;
                if (GetCommandType(tLine) != CommandType.Method)
                    continue;
                var name = tLine.Split(new[] { ' ', '[' }, StringSplitOptions.RemoveEmptyEntries)[1].Split('[').First();
                if (!IsNameValid(name))
                    throw new InvalidNameException(GlobalProperties.LineIndex, name);
                MethodList.Add(new Method(name, GetMethodParameters(tLine), CurrentMethodLabel));
            }
            GlobalProperties.LineIndex = 0;
        }

        #region Properties

        /// <summary>
        ///     The list of the blocks the parser is currently in
        /// </summary>
        private static List<Block> _blockList;

        /// <summary>
        ///     A list of the currently existing variables
        /// </summary>
        private static List<Variable> _variableList;

        /// <summary>
        ///     A list of all the methods existing in the code
        /// </summary>
        public static List<Method> MethodList;

        /// <summary>
        ///     The current method the parser is in
        /// </summary>
        private static Method _currentMethod;

        /// <summary>
        ///     the current byte address
        /// </summary>
        private static Address _byteCounter;

        /// <summary>
        ///     The current bit address
        /// </summary>
        private static Address _bitCounter;

        /// <summary>
        ///     The current method counter
        /// </summary>
        private static int _methodCounter;

        /// <summary>
        ///     The current byte
        /// </summary>
        /// <remarks>
        ///     Increases the Byte counter
        /// </remarks>
        /// <exception cref="TooManyValuesException">Gets thrown when the normal ram is full</exception>
        private static Address CurrentByteAddress
        {
            get
            {
                _byteCounter = _byteCounter.NextAddress;
                if (_byteCounter == null)
                    throw new TooManyValuesException(GlobalProperties.LineIndex);
                return _byteCounter;
            }
        }

        /// <summary>
        ///     The current method label
        /// </summary>
        /// <remarks>
        ///     Increases the method counter
        /// </remarks>
        private static Label CurrentMethodLabel
        {
            get
            {
                _methodCounter++;
                return new Label($"M{_methodCounter}");
            }
        }

        /// <summary>
        ///     The current bit address
        /// </summary>
        /// <remarks>
        ///     Increases the bit counter
        /// </remarks>
        private static Address CurrentBitAddress
        {
            get
            {
                _bitCounter = _bitCounter.NextAddress;
                if (_bitCounter == null)
                    throw new TooManyValuesException(GlobalProperties.LineIndex);
                return _bitCounter;
            }
        }

        private static List<InterruptType> _usedInterrupts;

        #endregion

        #region General

        /// <summary>
        ///     Gets wether the name (for a variable or a method) is valid
        /// </summary>
        /// <param name="name">The name that shall get checked</param>
        /// <returns>Wether it's valid</returns>
        private static bool IsNameValid(string name) => (name.Length > 0) && GlobalProperties.InvalidNames.All(
                                                            s =>
                                                                !s.Equals(name,
                                                                    StringComparison.CurrentCultureIgnoreCase)) &&
                                                        char.IsLetter(name[0]);

        #endregion

        #region Get

        #region Declaration

        /// <summary>
        ///     The declaration for the given variable name
        /// </summary>
        /// <param name="variable">The variable for the declaration</param>
        /// <param name="tLine">The line in which the variable is present</param>
        /// <returns>The declaration</returns>
        /// <exception cref="InvalidNameException">Gets thrown when the name of the variable isn't valid</exception>
        private static Declaration GetDeclarationToVariable(Variable variable, string tLine)
        {
            if (_variableList.Any(
                    var =>
                        var.Name
                            .Equals(variable.Name, StringComparison.CurrentCultureIgnoreCase)))
                throw new VariableExistsException(GlobalProperties.LineIndex, variable.Name);
            if (!IsNameValid(variable.Name))
                throw new InvalidNameException(GlobalProperties.LineIndex, variable.Name);
            _variableList.Add(variable);
            if (_blockList.Count > 0)
                _blockList.Last().Variables.Add(variable);
            else
                _currentMethod?.Variables.Add(variable);
            if (!(variable is Collection))
                return new Declaration(GetDeclarationAssignment(tLine));
            var s = tLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length != 2)
                throw new ParameterException(GlobalProperties.LineIndex, s.Length > 2 ? s[2] : s.LastOrDefault());
            return new Declaration();
        }

        /// <summary>
        ///     The assignment to a declaration
        /// </summary>
        /// <param name="line">The line of the declaration</param>
        /// <returns>The assignment</returns>
        /// <exception cref="ParameterException">Is thrown when after the declaration is something else than an assignment</exception>
        private static Assignment GetDeclarationAssignment(string line)
        {
            var splitted = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (!line.Contains(":=") && !line.Contains("+=") && !line.Contains("-=") && !line.Contains("*=") &&
                !line.Contains("/=") && !line.Contains("%=") && !line.Contains("&=") && !line.Contains("|="))
            {
                if (splitted.Length == 2)
                    return null;
                throw new ParameterException(GlobalProperties.LineIndex,
                    splitted.Length > 2
                        ? splitted[1]
                        : line);
            }
            var l =
                line.Substring(splitted.First().Length).Trim(' ');
            return GetAssignment(l, GetCommandType(l));
        }

        #endregion

        #region Parameter

        /// <summary>
        ///     Gets the parameters for a method
        /// </summary>
        /// <param name="line">The line in which the method is</param>
        /// <returns>A list of the parameters</returns>
        private static List<Variable> GetMethodParameters(string line)
        {
            var content = GetStringBetween('[', ']', line);
            var parameterAsStrings =
                content.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim(' '));
            var fin = new List<Variable>();
            foreach (var parameterAsString in parameterAsStrings)
                switch (GetCommandType(parameterAsString))
                {
                    case CommandType.Int:
                        {
                            var i = new Int(CurrentByteAddress, GetVariableDefinitionName(parameterAsString),
                                false);
                            CheckName(i.Name);
                            fin.Add(i);
                            break;
                        }
                    case CommandType.Cint:
                        {
                            var ci = new Cint(CurrentByteAddress, GetVariableDefinitionName(parameterAsString),
                                false);
                            CheckName(ci.Name);
                            fin.Add(ci);
                            break;
                        }
                    case CommandType.Char:
                        {
                            var c = new Char(CurrentByteAddress, GetVariableDefinitionName(parameterAsString),
                                false);
                            CheckName(c.Name);
                            fin.Add(c);
                            break;
                        }
                    case CommandType.Bool:
                        {
                            var b = new Bool(CurrentBitAddress, GetVariableDefinitionName(parameterAsString),
                                false);
                            CheckName(b.Name);
                            fin.Add(b);
                            break;
                        }
                    default:
                        throw new ParameterException(GlobalProperties.LineIndex, "Invalid Parameter type!");
                }
            return fin;
        }

        /// <summary>
        /// Checks the given name and throws an exception if it wasn't valid
        /// </summary>
        /// <param name="name">The name to check</param>
        private static void CheckName(string name)
        {
            if (
                _variableList.Any(
                    variable =>
                        variable.Name
                            .Equals(name, StringComparison.CurrentCultureIgnoreCase)))
                throw new VariableExistsException(GlobalProperties.LineIndex, name);
            if (!IsNameValid(name))
                throw new InvalidNameException(GlobalProperties.LineIndex, name);
        }

        /// <summary>
        ///     Gets the parameter values for the method call
        /// </summary>
        /// <param name="line">The line in which the method call is</param>
        /// <param name="parameters">A list of the necessary parameters</param>
        /// <returns>A list of the variable parameter values / the variable calls for the parameters</returns>
        public static List<VariableCall> GetMethodParameterValues(string line, IReadOnlyList<Variable> parameters)
        {
            var fin = new List<VariableCall>();
            var rawValues =
                GetStringBetween('[', ']', line)
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();
            if (rawValues.Count != parameters.Count)
                throw new ParameterException(GlobalProperties.LineIndex, "Wrong parameter count!");
            for (var index = 0; index < rawValues.Count; index++)
            {
                var value = rawValues[index];
                var v = new TemporaryParsedStringOperation(value).GeTemporaryReturning().Item2?.GetReturningCommand() as VariableCall;
                var parameter = parameters[index];
                if ((v == null) || (parameter is ByteVariable && v is BitVariableCall) ||
                    (parameter is BitVariable && v is ByteVariableCall))
                    throw new ParameterException(GlobalProperties.LineIndex, "Wrong parameter type");
                fin.Add(v);
            }
            return fin;
        }

        /// <summary>
        ///     Gets the parameter for the assignment
        /// </summary>
        /// <param name="tLine">The line in which the assignment is</param>
        /// <param name="splitter">The splitter with which the assignment is divided ( := / += / ... ) </param>
        /// <returns>A tuple of the parameters</returns>
        private static Tuple<Variable, ReturningCommand> GetAssignmentParameter(string tLine, string splitter)
        {
            var splitted = Strings.Split(tLine, splitter).ToArray();
            splitted = splitted.Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
            var variable = GetVariable(splitted[0]);
            if (splitted.Length != 2)
                throw new ParameterException(GlobalProperties.LineIndex, splitted.Length > 2 ? splitted[2] : splitted.LastOrDefault());
            if (variable == null || !variable.IsConstant && string.IsNullOrEmpty(variable.Address.ToString()))
                throw new InvalidNameException(GlobalProperties.LineIndex, splitted[0]);
            return new Tuple<Variable, ReturningCommand>(variable, GetReturningCommand(splitted[1]));
        }

        /// <summary>
        ///     Gets the limit for the Fortil block
        /// </summary>
        /// <param name="line">The line in which the beginning block is</param>
        /// <returns>The limit as a byte variable call</returns>
        /// <exception cref="InvalidCommandException">Gets thrown when there is a wrong parameter for the fortil block</exception>
        private static Tuple<ReturningCommand, ByteVariable> GetParameterForTil(string line)
        {
            var rawValues =
                GetStringBetween('[', ']', line)
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();
            if (rawValues.Count != 2)
                throw new ParameterException(GlobalProperties.LineIndex, rawValues.Count > 2 ? rawValues[2] : rawValues.LastOrDefault());
            var p = GetReturningCommand(rawValues[0]);
            if (p == null)
                throw new ParameterException(GlobalProperties.LineIndex, rawValues[1]);
            return new Tuple<ReturningCommand, ByteVariable>(p,
                new Int(CurrentByteAddress, rawValues[1], false));
        }

        #endregion

        /// <summary>
        ///     The assignment to the given type
        /// </summary>
        /// <param name="tLine">The line of the assignment</param>
        /// <param name="ct">The type of the assignment</param>
        /// <returns>The assignment</returns>
        private static Assignment GetAssignment(string tLine, CommandType ct)
        {
            switch (ct)
            {
                case CommandType.Assignment:
                    {
                        var pars = GetAssignmentParameter(tLine, ":=");
                        ThrowExceptionIfTypeUnEqualAssignment(pars);
                        return new Assignment(pars.Item1, pars.Item2);
                    }
                case CommandType.AddAssignment:
                    {
                        var pars = GetAssignmentParameter(tLine, "+=");
                        if (pars.Item1 is BitVariable)
                            throw new InvalidVariableTypeException(GlobalProperties.LineIndex, pars.Item1.Name);
                        ThrowExceptionIfTypeUnEqualAssignment(pars);
                        return new AddAssignment(pars.Item1, pars.Item2);
                    }
                case CommandType.SubtractAssignment:
                    {
                        var pars = GetAssignmentParameter(tLine, "-=");
                        if (pars.Item1 is BitVariable)
                            throw new InvalidVariableTypeException(GlobalProperties.LineIndex, pars.Item1.Name);
                        ThrowExceptionIfTypeUnEqualAssignment(pars);
                        return new SubtractAssignment(pars.Item1, pars.Item2);
                    }
                case CommandType.MultiplyAssignment:
                    {
                        var pars = GetAssignmentParameter(tLine, "*=");
                        if (pars.Item1 is BitVariable)
                            throw new InvalidVariableTypeException(GlobalProperties.LineIndex, pars.Item1.Name);
                        ThrowExceptionIfTypeUnEqualAssignment(pars);
                        return new MultiplyAssignment(pars.Item1, pars.Item2);
                    }
                case CommandType.DivideAssignment:
                    {
                        var pars = GetAssignmentParameter(tLine, "/=");
                        if (pars.Item1 is BitVariable)
                            throw new InvalidVariableTypeException(GlobalProperties.LineIndex, pars.Item1.Name);
                        ThrowExceptionIfTypeUnEqualAssignment(pars);
                        return new DivideAssignment(pars.Item1, pars.Item2);
                    }
                case CommandType.ModuloAssignment:
                    {
                        var pars = GetAssignmentParameter(tLine, "%=");
                        if (pars.Item1 is BitVariable)
                            throw new InvalidVariableTypeException(GlobalProperties.LineIndex, pars.Item1.Name);
                        ThrowExceptionIfTypeUnEqualAssignment(pars);
                        return new ModuloAssignment(pars.Item1, pars.Item2);
                    }
                case CommandType.AndAssignment:
                    {
                        var pars = GetAssignmentParameter(tLine, "&=");
                        ThrowExceptionIfTypeUnEqualAssignment(pars);
                        return new AndAssignment(pars.Item1, pars.Item2);
                    }
                case CommandType.OrAssignment:
                    {
                        var pars = GetAssignmentParameter(tLine, "|=");
                        ThrowExceptionIfTypeUnEqualAssignment(pars);
                        return new OrAssignment(pars.Item1, pars.Item2);
                    }
                default:
                    return null;
            }
        }

        private static string GetTCodeWithInsertedSpaces(string code)
        {
            char? previousChar = null;
            char? previousVisibleChar = null;
            var fin = "";
            var signs =
                GlobalProperties.AssignmentSigns.Concat(
                        GlobalProperties.OperationPriorities.Select(priority => priority.OperationSign))
                    .Concat(new List<string> {"(", ")"})
                    .ToList();
            var currentLineTillThere = "";

            for (var index = 0; index < code.Length; index++)
            {
                var currentChar = code[index];
                var nextChar = index < code.Length - 1 ? (char?) code[index + 1] : null;

                var replaced = false;

                foreach (var sign in signs)
                {
                    if (sign.Length == 1 &&
                        currentChar == sign.FirstOrDefault() &&
                        (currentChar != '-' || 
                        (previousChar == null || previousChar == ']' || previousChar == ')' || !char.IsSymbol(previousChar.Value) && !char.IsPunctuation(previousChar.Value)) &&
                        (nextChar == null || nextChar == '[' || nextChar == '(' || !char.IsSymbol(nextChar.Value) && !char.IsPunctuation(nextChar.Value))) &&
                        signs.All(priority => sign.FirstOrDefault() != previousVisibleChar) &&
                        (currentChar != ':' && currentChar != '.' || GlobalProperties.AssignmentSigns.Any(s => currentLineTillThere.Contains(s))))
                    {
                        fin += $" {currentChar} ";
                        currentLineTillThere += currentChar.ToString();
                        replaced = true;
                        break;
                    }
                    if (sign.Length != 2 || nextChar == null ||
                        currentChar != sign[0] ||
                        nextChar.Value != sign[1])
                        continue;

                    fin += $" {currentChar}{nextChar} ";
                    currentLineTillThere += currentChar.ToString() + nextChar;
                    index++;
                    replaced = true;
                    break;
                }

                if (!replaced)
                {
                    fin += currentChar.ToString();
                    if (currentChar == '\n')
                        currentLineTillThere = "";
                    else
                        currentLineTillThere += currentChar.ToString();
                }

                previousChar = currentChar;
                if (!char.IsWhiteSpace(currentChar))
                    previousVisibleChar = currentChar;
            }
            return fin;
        }

        private static void ThrowExceptionIfTypeUnEqualAssignment(Tuple<Variable, ReturningCommand> pars)
        {
            var t1 = pars.Item1.GetType();
            var t2 = pars.Item2 is ByteVariableCall
                ? typeof(ByteVariable)
                : (pars.Item2 is BitVariableCall ? typeof(BitVariable) : null);
            if ((t2 != null) && (t1 == t2))
                throw new InvalidVariableTypeException(GlobalProperties.LineIndex,
                    pars.Item1?.Name ??
                    ((VariableCall) pars.Item2)?.Variable?.Name ?? pars.Item2?.ToString());
        }

        /// <summary>
        ///     Gets the command type to the given code line
        /// </summary>
        /// <param name="tLine">The line to which the the command type will get evaluated</param>
        /// <returns>The type</returns>
        private static CommandType GetCommandType(string tLine)
        {
            if (tLine.Length == 0)
                return CommandType.VariableConstantMethodCallOperationOrNothing;
            var splitted = tLine.Split(new[] { ' ', '[', '#' }, StringSplitOptions.RemoveEmptyEntries);
            switch (splitted.FirstOrDefault())
            {
                case "int":
                    return CommandType.Int;
                case "if":
                    return CommandType.IfBlock;
                case "else":
                    return CommandType.ElseBlock;
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
                case "collection":
                    return CommandType.Collection;
                case "return":
                    return CommandType.Return;
                case "method":
                    return CommandType.Method;
                case "isrexternal0":
                case "isrexternal1":
                case "isrtimer0":
                case "isrcounter0":
                case "isrtimer1":
                case "isrcounter1":
                    return CommandType.InterruptServiceRoutine;
                case "endisr":
                case "endmethod":
                    return CommandType.EndMethod;
                case "sleep":
                    return CommandType.Sleep;
                default:
                    if(splitted.Length < 2)
                        return CommandType.VariableConstantMethodCallOperationOrNothing;
                    switch (splitted[1])
                    {
                        case ":=":
                            return CommandType.Assignment;
                        case "+=":
                            return CommandType.AddAssignment;
                        case "-=":
                            return CommandType.SubtractAssignment;
                        case "*=":
                            return CommandType.MultiplyAssignment;
                        case "/=":
                            return CommandType.DivideAssignment;
                        case "%=":
                            return CommandType.ModuloAssignment;
                        case "&=":
                            return CommandType.AndAssignment;
                        case "|=":
                            return CommandType.OrAssignment;
                        default:
                            return CommandType.VariableConstantMethodCallOperationOrNothing;
                    }
            }
        }

        /// <summary>
        ///     Gets the variable to the given identifier
        /// </summary>
        /// <param name="variableIdentifier">The identifier to identify the variable</param>
        /// <returns>The suitable variable</returns>
        public static Variable GetVariable(string variableIdentifier)
        {
            var splitted = variableIdentifier.Split(new[] { '.', ':' }, StringSplitOptions.RemoveEmptyEntries);
            var var = _variableList.FirstOrDefault(
                variable =>
                    string.Equals(variable.Name, splitted.First(),
                        StringComparison.CurrentCultureIgnoreCase));

            if (!variableIdentifier.Contains('.') && !variableIdentifier.Contains(':'))
                return var;

            var index =
                new TemporaryParsedStringOperation(splitted.LastOrDefault()).GeTemporaryReturning().Item2?.GetReturningCommand() as
                    ByteVariableCall;
            if (index == null)
                throw new ParameterException(GlobalProperties.LineIndex, splitted.LastOrDefault() ?? variableIdentifier);

            return variableIdentifier.Contains('.')
                ? new BitOfVariable(var?.Address, index.ByteVariable, GlobalProperties.Label, GlobalProperties.Label,
                    GlobalProperties.Label, GlobalProperties.Label, GlobalProperties.Label, GlobalProperties.Label,
                    GlobalProperties.Label, GlobalProperties.Label)
                : (Variable) new VariableOfCollectionVariable((Collection) var, index);
        }

        /// <summary>
        ///     Gets the name of the variable (in a declaration)
        /// </summary>
        /// <param name="line">The line of the declaration</param>
        /// <returns>The name</returns>
        private static string GetVariableDefinitionName(string line)
        {
            var s = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length < 2)
                throw new ParameterException(GlobalProperties.LineIndex,
                    s.LastOrDefault() ?? line);
            return s[1];
        }

        /// <summary>
        ///     Gets the condition (for if/while)
        /// </summary>
        /// <param name="line">The line in which the condition is</param>
        /// <returns>The condition</returns>
        private static Condition GetCondition(string line)
        {
            if (line.TakeWhile(c => c != ']').Count() != line.Length - 1)
                throw new ParameterException(GlobalProperties.LineIndex, line.Split(']').Length > 1 ? line.Split(']')[1] : line);
            return new Condition(GetReturningCommand(GetStringBetween('[', ']', line)));
        }

        /// <summary>
        ///     Returns the string between to identifier
        /// </summary>
        /// <param name="start">The start splitter</param>
        /// <param name="end">The end splitter</param>
        /// <param name="line">The line that shall get splitted</param>
        /// <returns>The string between</returns>
        private static string GetStringBetween(char start, char end, string line)
        {
            var sb = new StringBuilder();
            if (!(line.Contains(start) && line.Contains(end)))
                throw new InvalidSyntaxException(GlobalProperties.LineIndex);
            foreach (var s in line.Split(start)[1])
            {
                if (s == end)
                    break;
                sb.Append(s);
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Gets the returning command to the given code line
        /// </summary>
        /// <param name="line">The line of the returning command</param>
        /// <returns>The returning command</returns>
        /// <exception cref="InvalidNameException">
        ///     Gets thrown when nothing suitable was found - this is normally caused by a wrong
        ///     name
        /// </exception>
        public static ReturningCommand GetReturningCommand(string line)
        {
            var fin = new TemporaryParsedStringOperation(line).GeTemporaryReturning().Item2?.GetReturningCommand();
            if (fin != null)
                return fin;
            throw new InvalidNameException(GlobalProperties.LineIndex, line);
        }

        #endregion
    }
}