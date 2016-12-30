using System.Collections.Generic;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    public class Method : Command
    {
        public readonly List<Variable.Variable> Variables;

        public Method(string name)
        {
            Variables = new List<Variable.Variable>();
            Name = name;
        }

        public string Name { get; }
    }
}