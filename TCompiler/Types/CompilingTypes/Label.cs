namespace TCompiler.Types.CompilingTypes
{
    /// <summary>
    /// Represents a normal label.
    /// </summary>
    /// <remarks>Actually you can't directly type in a label in assembler, so it actually isn't a normal command, but it's still used in stuff like loops</remarks>
    public class Label : Command
    {
        /// <summary>
        /// Initiates a new Label
        /// </summary>
        /// <returns>Nothing</returns>
        public Label(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the label
        /// </summary>
        /// <value>The name as a string</value>
        public string Name { get; }

        /// <summary>
        /// The stuff that's called when the label is used in a jmp command
        /// </summary>
        /// <remarks>There's the stuff with sjmp and so on because jb, jnb, ... can't jump that far, so they all have to get replaced with ljmp</remarks>
        /// <returns>The string that must get inserted</returns>
        public override string ToString() => Name;  //TODO change this to relative jumps
                                                    /// <summary>
                                                    /// The label mark that'll be present in assembler
                                                    /// </summary>
                                                    /// <returns>The string of the label mark (like L0:)</returns>
        public string LabelMark() => $"{Name}:";
    }
}