#region

using System.Linq;
using TCompiler.Compiling;
using TCompiler.Types.CheckTypes.TCompileException;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    /// The base class for every variable
    /// </summary>
    public abstract class Variable
    {
        /// <summary>
        /// Initializes a new variable
        /// </summary>
        /// <param name="address">The address of the variable when it isn't constant</param>
        /// <param name="name">The name of the variable to use in T when it isn't constant</param>
        /// <param name="isConstant">Indicates wether the value is constant, so that it's saved in the value property of inheriting types</param>
        protected Variable(string address, string name, bool isConstant)
        {
            if (!isConstant &&
                (string.IsNullOrEmpty(name) ||
                 (name.Any(c => !char.IsLetterOrDigit(c) && (c != '-') && (c != '_') && (c != '.')) &&
                  char.IsLetter(name[0]))))
                throw new InvalidNameException(ParseToObjects.Line, name);

            Name = name;
            IsConstant = isConstant;
            Address = address;
        }

        /// <summary>
        /// The name of the variable to use in T when it isn't constant
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Indicates wether the value is constant, so that it's saved in the value property of inheriting types
        /// </summary>
        public bool IsConstant { get; }
        /// <summary>
        /// The address of the variable when it isn't constant
        /// </summary>
        public string Address { get; } //Byte: 0; Bit: 0.0

        /// <summary>
        /// Gets the variable value
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString() => Address;
    }
}