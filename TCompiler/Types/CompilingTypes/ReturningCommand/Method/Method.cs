#region

using System.Collections.Generic;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    public class Method : Command
    {
        public Method(string name, List<Variable.Variable> parameters, string label)
        {
            Variables = new List<Variable.Variable>();
            Name = name;
            Parameters = parameters;
            Label = label;
        }

        public List<Variable.Variable> Variables { get; }
        public List<Variable.Variable> Parameters { get; }
        public string Label { get; }

        private string Name { get; }
        public string GetName() => Name;
    }
}