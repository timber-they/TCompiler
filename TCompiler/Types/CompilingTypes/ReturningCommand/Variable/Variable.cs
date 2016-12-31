using System.Linq;
using TCompiler.Compiling;
using TCompiler.Types.CheckTypes.TCompileException;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public abstract class Variable : Command
    {
        protected Variable(bool isConstant, string name = null)
        {
            if (!isConstant &&
                (string.IsNullOrEmpty(name) ||
                 (name.Any(c => !char.IsLetterOrDigit(c) && (c != '-') && (c != '_')) && char.IsLetter(name[0]))))
                throw new InvalidNameException(ParseToObjects.Line);

            Name = name;
            IsConstant = isConstant;
        }

        public string Name { get; }
        public bool IsConstant { get; }

        public override string ToString() => Name;
    }
}