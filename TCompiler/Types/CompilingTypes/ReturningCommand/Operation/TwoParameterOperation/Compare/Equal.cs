#region

using System.Text;

using TCompiler.AssembleHelp;
using TCompiler.Settings;
using TCompiler.Types.CompilerTypes;

#endregion


namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare
{
    /// <summary>
    ///     Indicates wether two integral values are equal<br />
    ///     Syntax:<br />
    ///     paramA = paramB
    /// </summary>
    public class Equal : Compare
    {
        /// <summary>
        ///     Initializes a new Equal operation
        /// </summary>
        /// <param name="paramA">The first parameter to compare</param>
        /// <param name="paramB">The second parameter to compare</param>
        /// <param name="cLine">The original T code line</param>
        public Equal (ReturningCommand paramA, ReturningCommand paramB, CodeLine cLine) :
            base (paramA, paramB, cLine) {}

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make an equal comparison
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString ()
        {
            var notequal = GlobalProperties.Label;
            var end      = GlobalProperties.Label;

            var sb = new StringBuilder ();
            sb.AppendLine (AssembleCodePreviews.MoveParametersIntoAb (ParamA, ParamB));
            sb.AppendLine ($"cjne A, 0F0h, {notequal.DestinationName}");
            sb.AppendLine ($"{Ac.SetBit} 0E0h.0");
            sb.AppendLine ($"{Ac.Jump} {end.DestinationName}");
            sb.AppendLine (notequal.LabelMark ());
            sb.AppendLine ($"{Ac.Clear} 0E0h.0");
            sb.AppendLine (end.LabelMark ());

            return sb.ToString ();
        }
    }
}