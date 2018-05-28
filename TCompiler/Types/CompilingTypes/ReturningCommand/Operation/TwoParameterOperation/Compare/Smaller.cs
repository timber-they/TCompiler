#region

using System.Text;

using TCompiler.AssembleHelp;
using TCompiler.Settings;
using TCompiler.Types.CompilerTypes;

#endregion


namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare
{
    /// <summary>
    ///     Indicates wether paramA is smaller than paramB<br />
    ///     Syntax:<br />
    ///     paramA &#60; paramB
    /// </summary>
    public class Smaller : Compare
    {
        /// <summary>
        ///     Initializes a new Smaller operation
        /// </summary>
        /// <param name="paramA">The smaller parameter</param>
        /// <param name="paramB">The bigger or equal parameter</param>
        /// <param name="cLine">The original T code line</param>
        public Smaller (ReturningCommand paramA, ReturningCommand paramB, CodeLine cLine) :
            base (paramA, paramB, cLine) {}

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a smaller comparison
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString ()
        {
            var notequal = GlobalProperties.Label;
            var end      = GlobalProperties.Label;
            var bigger   = GlobalProperties.Label;

            var sb = new StringBuilder ();
            sb.AppendLine (AssembleCodePreviews.MoveParametersIntoAb (ParamA, ParamB));
            sb.AppendLine ($"cjne A, 0F0h, {notequal.DestinationName}");
            sb.AppendLine ($"{Ac.Clear} 0E0h.0");
            sb.AppendLine ($"{Ac.Jump} {end.DestinationName}");
            sb.AppendLine (notequal.LabelMark ());
            sb.AppendLine ($"jnc {bigger.DestinationName}");
            sb.AppendLine ($"{Ac.SetBit} 0E0h.0");
            sb.AppendLine ($"{Ac.Jump} {end.DestinationName}");
            sb.AppendLine (bigger.LabelMark ());
            sb.AppendLine ($"{Ac.Clear} 0E0h.0");
            sb.AppendLine (end.LabelMark ());

            return sb.ToString ();
        }
    }
}