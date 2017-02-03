using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TCompiler.Compiling;
using TCompiler.General;
using TCompiler.Settings;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes.ReturningCommand.Method;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;
using Char = TCompiler.Types.CompilingTypes.ReturningCommand.Variable.Char;

namespace TCompiler.Types.CompilingTypes.TemporaryOperation.TemporaryReturning
{
    /// <summary>
    /// Represents a temporary variable, constant, method call or nothing returning command
    /// </summary>
    public class TemporaryVariableConstantMethodCallOrNothing : ITemporaryReturning
    {
        /// <summary>
        /// Initializes a new temporaryVariableConstantMethodCallOrNothing returning command
        /// </summary>
        /// <param name="value">The value of the expression as a string</param>
        public TemporaryVariableConstantMethodCallOrNothing(string value)
        {
            Value = value;
        }

        /// <summary>
        /// The value of the expression as a string
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Evaluates the returning command of the TemporaryVariableConstantMethodCallOrNothing TemporaryReturning
        /// </summary>
        /// <returns>The returning command as a ReturningCommand</returns>
        public ReturningCommand.ReturningCommand GetReturningCommand()
        {
            if (string.IsNullOrEmpty(Value))
                return null;

            var method = GetMethod(Value, ParseToObjects.MethodList);
            if (method != null)
            {
                var values = ParseToObjects.GetMethodParameterValues(Value, method.Parameters);
                return new MethodCall(method, values);
            }

            var variable = ParseToObjects.GetVariable(Value);
            if ((variable != null) && !(variable is BitOfVariable))
            {
                var byteVariable = variable as ByteVariable;
                if (byteVariable != null)
                    return new ByteVariableCall(byteVariable);
                return new BitVariableCall((BitVariable) variable);
            }

            bool b;
            if (bool.TryParse(Value, out b))
                return new BitVariableCall(new Bool(null, null, true, b));

            uint ui; //TODO check if value should be cint
            if ((Value.StartsWith("0x") &&
                 uint.TryParse(Strings.Trim("0x", Value), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out ui)) ||
                uint.TryParse(Value, NumberStyles.None, CultureInfo.CurrentCulture, out ui))
            {
                if (ui > 255)
                    throw new InvalidValueException(GlobalProperties.LineIndex, ui.ToString());
                return new ByteVariableCall(new Int(null, null, true, Convert.ToByte(ui)));
            }

            int i;
            if (!Value.Contains('.') &&
                (Value.StartsWith("0x") && int.TryParse(Strings.Trim("0x", Value), 0 << 9, CultureInfo.CurrentCulture, out i) ||
                 int.TryParse(Value, NumberStyles.Number, CultureInfo.CurrentCulture, out i)))
            {
                if ((i >= 0x80) || (i < -0x80))
                    throw new InvalidValueException(GlobalProperties.LineIndex, i.ToString());
                return new ByteVariableCall(new Cint(null, null, true, (byte) Convert.ToSByte(i)));
            }

            char c;
            if (Value.StartsWith("'") && Value.EndsWith("'") && char.TryParse(Value.Trim('\''), out c))
                return new ByteVariableCall(new Char(null, null, true, (byte) c));

            return null;
        }

        /// <summary>
        ///     Gets the method to the given name of the method
        /// </summary>
        /// <param name="methodName">The name of the method</param>
        /// <param name="methodList"></param>
        /// <returns>The method</returns>
        private static Method GetMethod(string methodName, IEnumerable<Method> methodList)
            =>
            methodList.FirstOrDefault(
                method =>
                    string.Equals(method.Name, methodName.Split(new[] { ' ', ']' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(),
                        StringComparison.CurrentCultureIgnoreCase));
    }
}