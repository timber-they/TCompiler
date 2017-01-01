using System.Collections.Generic;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    public class Method : Command
    {
        public List<Variable.Variable> Variables { get; }
        public List<Variable.Variable> Parameters { get; }

        public Method(string name, List<Variable.Variable> parameters)
        {
            Variables = new List<Variable.Variable>();
            Name = name;
            Parameters = parameters;
        }

        public string Name { get; }
    }
}