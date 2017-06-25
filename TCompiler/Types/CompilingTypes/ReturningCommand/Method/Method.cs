#region

using System.Collections.Generic;

using TCompiler.Types.CompilerTypes;

#endregion


namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    /// <summary>
    ///     A method that can be called
    /// </summary>
    /// <remarks>It's of the form method m [int i, bool d]</remarks>
    public class Method : Command
    {
        /// <summary>
        ///     Initiates a new method command
        /// </summary>
        /// <returns>Nothing</returns>
        /// <param name="name">The name of the method</param>
        /// <param name="parameters">the list of the parameters</param>
        /// <param name="label">The label of the method</param>
        /// <param name="tCode">The original T code line</param>
        public Method (string name, List<Variable.Variable> parameters, Label label, CodeLine tCode) : base (true, true,
                                                                                                             tCode)
        {
            Variables = new List<Variable.Variable> ();
            Name = name;
            Parameters = parameters;
            Label = label;
        }

        /// <summary>
        ///     The variables that appear in the method. See block
        /// </summary>
        /// <value>A list of the variables.</value>
        public List<Variable.Variable> Variables { get; }

        /// <summary>
        ///     The parameters of the method
        /// </summary>
        /// <remarks>is used like [int i, bool d]</remarks>
        /// <value>A list of the parameters (variables)</value>
        public List<Variable.Variable> Parameters { get; }

        /// <summary>
        ///     The start of the method in assembler. The method is called with call LabelName
        /// </summary>
        /// <remarks>Mustn't be called with Label.ToString() !</remarks>
        /// <value>The label as a label.</value>
        public Label Label { get; }

        /// <summary>
        ///     The name of the method used in T
        /// </summary>
        /// <value>The name as a string</value>
        public string Name { get; }
    }
}