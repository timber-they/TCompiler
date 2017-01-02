using System.Collections.Generic;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    public class Method : Command
    {
        public List<Variable.Variable> Variables { get; }
        public List<Variable.Variable> Parameters { get; }
        public string Label { get; }

        public Method(string name, List<Variable.Variable> parameters, string label)
        {
            Variables = new List<Variable.Variable>();
            Name = name;
            Parameters = parameters;
            Label = label;
        }

        private string Name { get; }
        public string GetName() => Name;
    }
}