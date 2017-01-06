#region

using System.Linq;
using TCompiler.Compiling;
using TCompiler.Types.CheckTypes.TCompileException;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public abstract class Variable
    {
        protected Variable(bool isConstant, string address, string name)
        {
            if (!isConstant &&
                (string.IsNullOrEmpty(name) ||
                 (name.Any(c => !char.IsLetterOrDigit(c) && (c != '-') && (c != '_') && (c != '.')) &&
                  char.IsLetter(name[0]))))
                throw new InvalidNameException(ParseToObjects.Line);

            Name = name;
            IsConstant = isConstant;
            Address = address;
        }

        private string Name { get; }
        public bool IsConstant { get; }
        public string Address { get; } //Byte: 0; Bit: 0.0

        public override string ToString() => Address;

        public string GetName() => Name;
    }
}