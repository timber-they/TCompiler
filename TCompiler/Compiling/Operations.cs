using System;
using System.Linq;
using TCompiler.Enums;
using TCompiler.General;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes.ReturningCommand;
using TCompiler.Types.CompilingTypes.ReturningCommand.Method;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Compiling
{
    public static class Operations
    {

        /// <summary>
        ///     Gets the OperationObject to the given type & line
        /// </summary>
        /// <param name="ct">The type of the operation</param>
        /// <param name="line">The line in which the operation is standing</param>
        /// <returns>The operation</returns>
        /// <exception cref="ParameterException">Gets thrown when the operation has invalid parameters</exception>
        public static Operation GetOperation(CommandType ct, string line)
        {
            Tuple<ReturningCommand, VariableCall> vars;
            switch (ct)
            {
                case CommandType.And:
                    return new And(GetOperationParametersWithDivider('&', line));
                case CommandType.Not:
                    return new Not(GetParameter('!', line));
                case CommandType.Or:
                    return new Or(GetOperationParametersWithDivider('|', line));
                case CommandType.Add:
                    vars = GetOperationParametersWithDivider('+', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new InvalidVariableTypeException(ParseToObjects.LineIndex, vars.Item2.Variable.Name);
                    return new Add(vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Subtract:
                    vars = GetOperationParametersWithDivider('-', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new InvalidVariableTypeException(ParseToObjects.LineIndex, vars.Item2.Variable.Name);
                    return new Subtract(vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Multiply:
                    vars = GetOperationParametersWithDivider('*', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new InvalidVariableTypeException(ParseToObjects.LineIndex, vars.Item2.Variable.Name);
                    return new Multiply(vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Divide:
                    vars = GetOperationParametersWithDivider('/', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new InvalidVariableTypeException(ParseToObjects.LineIndex, vars.Item2.Variable.Name);
                    return new Divide(vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Modulo:
                    vars = GetOperationParametersWithDivider('%', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new InvalidVariableTypeException(ParseToObjects.LineIndex, vars.Item2.Variable.Name);
                    return new Modulo(vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Bigger:
                    vars = GetOperationParametersWithDivider('>', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new InvalidVariableTypeException(ParseToObjects.LineIndex, vars.Item2.Variable.Name);
                    return new Bigger(vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Smaller:
                    vars = GetOperationParametersWithDivider('<', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new InvalidVariableTypeException(ParseToObjects.LineIndex, vars.Item2.Variable.Name);
                    return new Smaller(vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Equal:
                    vars = GetOperationParametersWithDivider('=', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new InvalidVariableTypeException(ParseToObjects.LineIndex, vars.Item2.Variable.Name);
                    return new Equal(vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.UnEqual:
                    vars = GetOperationParametersWithDivider("!=", line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new InvalidVariableTypeException(ParseToObjects.LineIndex, vars.Item2.Variable.Name);
                    return new UnEqual(vars.Item1, (ByteVariableCall) vars.Item2);
                case CommandType.Increment:
                    try
                    {
                        return new Increment((ByteVariableCall) GetParameter("++", line));
                    }
                    catch (InvalidCastException)
                    {
                        throw new ParameterException(ParseToObjects.LineIndex, line);
                    }
                case CommandType.Decrement:
                    try
                    {
                        return new Decrement((ByteVariableCall) GetParameter("--", line));
                    }
                    catch (InvalidCastException)
                    {
                        throw new ParameterException(ParseToObjects.LineIndex, line);
                    }
                case CommandType.ShiftLeft:
                    vars = GetOperationParametersWithDivider("<<", line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new InvalidVariableTypeException(ParseToObjects.LineIndex, vars.Item2.Variable.Name);
                    return new ShiftLeft(vars.Item1, (ByteVariableCall) vars.Item2, ParseToObjects.CurrentRegister,
                        ParseToAssembler.Label);
                case CommandType.ShiftRight:
                    vars = GetOperationParametersWithDivider("<<", line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new InvalidVariableTypeException(ParseToObjects.LineIndex, vars.Item2.Variable.Name);
                    return new ShiftRight(vars.Item1, (ByteVariableCall) vars.Item2, ParseToObjects.CurrentRegister,
                        ParseToAssembler.Label);
                case CommandType.BitOf:
                    vars = GetOperationParametersWithDivider('.', line);
                    if (vars.Item2.GetType() != typeof(ByteVariableCall))
                        throw new InvalidVariableTypeException(ParseToObjects.LineIndex, vars.Item2.Variable.Name);
                    var bo = new BitOf(vars.Item1, (ByteVariableCall) vars.Item2,
                        ParseToAssembler.Label, ParseToAssembler.Label, ParseToAssembler.Label, ParseToObjects.CurrentRegister);
                    ParseToObjects.CurrentRegisterAddress--;
                    return bo;
                case CommandType.VariableOfCollection:

                    var ss = line.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
                    if (ss.Length != 2)
                        throw new ParameterException(ParseToObjects.LineIndex, ss.Length > 2 ? ss[2] : ss.LastOrDefault());
                    var collection = ParseToObjects.VariableList.FirstOrDefault(variable => variable.Name == ss[0]) as Collection;
                    var collectionIndex = ParseToObjects.GetVariableConstantMethodCallOrNothing(ss[1]) as ByteVariableCall;
                    if (collection == null || collectionIndex == null)
                        throw new InvalidCommandException(ParseToObjects.LineIndex, line);
                    return new VariableOfCollection(collection, collectionIndex);
                default:
                    return null;
            }
        }

        /// <summary>
        ///     Gets parameters for a twoParameterOperation
        /// </summary>
        /// <param name="divider">The divider of the operation</param>
        /// <param name="line">The line in which the operation is</param>
        /// <returns>A tuple of the parameters</returns>
        private static Tuple<ReturningCommand, VariableCall> GetOperationParametersWithDivider(char divider, string line)
        {
            var ss = line.Split(new[] { divider }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            var var1 = ParseToObjects.GetVariableConstantMethodCallOrNothing(ss.Last()) as VariableCall;
            ss.RemoveAt(ss.Count - 1);
            var head = string.Join($" {divider} ", ss);
            var var2 = ParseToObjects.GetReturningCommand(head);

            if (var1 == null || var2 == null)
                throw new ParameterException(ParseToObjects.LineIndex, var1 == null ? ss.Last() : head);
            return new Tuple<ReturningCommand, VariableCall>(var2, var1);
        }

        /// <summary>
        ///     Gets parameters for a twoParameterOperation
        /// </summary>
        /// <param name="divider">The divider of the operation as a string</param>
        /// <param name="line">The line in which the operation is</param>
        /// <returns>A tuple of the parameters</returns>
        private static Tuple<ReturningCommand, VariableCall> GetOperationParametersWithDivider(string divider, string line)
        {
            var ss = Strings.Split(line, divider, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            var var1 = ParseToObjects.GetVariableConstantMethodCallOrNothing(ss.Last()) as VariableCall;
            ss.RemoveAt(ss.Count - 1);
            var tail = string.Join($" {divider} ", ss);
            var var2 = ParseToObjects.GetReturningCommand(tail);

            if (var1 == null || var2 == null)
                throw new InvalidCommandException(ParseToObjects.LineIndex, line);
            return new Tuple<ReturningCommand, VariableCall>(var2, var1);
        }

        /// <summary>
        ///     Gets the parameter for an OneParameterOperation
        /// </summary>
        /// <param name="divider">The divider (The operation sign)</param>
        /// <param name="line">The line in which the operation is</param>
        /// <returns>The parameter</returns>
        private static VariableCall GetParameter(char divider, string line)
        {
            var ss = line.Trim(divider, ' ').Trim();
            if (ss.Contains(' '))
                throw new ParameterException(ParseToObjects.LineIndex, ss);
            var var1 = ParseToObjects.GetVariableConstantMethodCallOrNothing(ss);
            if (var1 == null)
                throw new InvalidCommandException(ParseToObjects.LineIndex, line);
            if (!(var1 is VariableCall))
                throw new ParameterException(ParseToObjects.LineIndex, (var1 as MethodCall)?.Method.Name ?? var1.ToString());
            var bitVariable = var1 as BitVariableCall;
            return bitVariable != null ? (VariableCall) bitVariable : (ByteVariableCall) var1;
        }

        /// <summary>
        ///     Gets the parameter for an OneParameterOperation
        /// </summary>
        /// <param name="divider">The divider (The operation sign) as a string</param>
        /// <param name="line">The line in which the operation is</param>
        /// <returns>The parameter</returns>
        private static VariableCall GetParameter(string divider, string line)
        {
            var ss = Strings.Trim(divider, line).Trim();
            if (ss.Contains(' '))
                throw new ParameterException(ParseToObjects.LineIndex, ss);
            var var1 = ParseToObjects.GetVariableConstantMethodCallOrNothing(ss);
            if (var1 == null)
                throw new InvalidCommandException(ParseToObjects.LineIndex, line);
            var bitVariable = var1 as BitVariableCall;
            if (!(var1 is VariableCall))
                throw new ParameterException(ParseToObjects.LineIndex, (var1 as MethodCall)?.Method.Name ?? var1.ToString());
            if (bitVariable != null)
                return bitVariable;
            return (ByteVariableCall) var1;
        }
    }
}