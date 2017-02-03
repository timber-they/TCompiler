namespace TCompiler.Types.CompilingTypes
{
    /// <summary>
    ///     Represents a normal label.
    /// </summary>
    /// <remarks>
    ///     Actually you can't directly type in a label in assembler, so it actually isn't a normal command, but it's
    ///     still used in stuff like loops
    /// </remarks>
    public class Label : Command
    {
        /// <summary>
        ///     The count for the help labels
        /// </summary>
        private static int HelpLabelCount { get; set; }

        /// <summary>
        ///     Initiates a new Label
        /// </summary>
        /// <returns>Nothing</returns>
        /// <param name="destinationName">The destination label name</param>
        public Label(string destinationName) : base(false, false)
        {
            DestinationName = destinationName;
            HelpLabelJumpName = $"j{HelpLabelCount}";
            HelpLabelEndName = $"e{HelpLabelCount}";
            HelpLabelCount++;
        }

        /// <summary>
        ///     Initiates a new Label with copying the old one
        /// </summary>
        /// <param name="old">The old label to copy from</param>
        public Label(Label old) : base(false, false)
        {
            DestinationName = old.DestinationName;
            HelpLabelJumpName = old.HelpLabelJumpName;
            HelpLabelEndName = old.HelpLabelEndName;
        }

        /// <summary>
        ///     The name of the label
        /// </summary>
        /// <value>The name as a string</value>
        public string DestinationName { get; }

        /// <summary>
        ///     The help label name to jump to when a jump is required
        /// </summary>
        /// <value>The name as a string</value>
        private string HelpLabelJumpName { get; set; }

        /// <summary>
        ///     The end of the whole jump
        /// </summary>
        /// <value>The name as a string</value>
        private string HelpLabelEndName { get; set; }

        /// <summary>
        ///     The stuff that's called when the label is used in a jmp command
        /// </summary>
        /// <remarks>
        ///     There's the stuff with help labels and so on because jb, jnb, ... can't jump that far, so they all have to get
        ///     replaced with ljmp
        /// </remarks>
        /// <returns>The string that must get inserted</returns>
        public override string ToString()
        {
            var res =
                $"{HelpLabelJumpName}\njmp {HelpLabelEndName}\n{HelpLabelJumpName}:\njmp {DestinationName}\n{HelpLabelEndName}:";
            HelpLabelJumpName = $"j{HelpLabelCount}";
            HelpLabelEndName = $"e{HelpLabelCount}";
            HelpLabelCount++;
            return res;
        }

        /// <summary>
        ///     The label mark that'll be present in assembler
        /// </summary>
        /// <returns>The string of the label mark (like L0:)</returns>
        public string LabelMark() => $"{DestinationName}:";
    }
}