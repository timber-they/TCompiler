#region

using System.Text;

using TCompiler.AssembleHelp;
using TCompiler.Types.CompilerTypes;

#endregion


namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     Evaluates paramA modulo paramB<br />
    ///     Syntax:<br />
    ///     paramA % paramB
    /// </summary>
    public class Modulo : TwoParameterOperation
    {
        /// <summary>
        ///     Initiates a new modulo operation
        /// </summary>
        /// <param name="paramA">The first modulo parameter</param>
        /// <param name="paramB">The second modulo parameter</param>
        /// <param name="cLine">The original T code line</param>
        public Modulo (ReturningCommand paramA, ReturningCommand paramB, CodeLine cLine) :
            base (paramA, paramB, cLine) {}

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a modulo operation
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString ()
        {
            var sb = new StringBuilder ();
            sb.AppendLine (AssembleCodePreviews.MoveParametersIntoAb (ParamA, ParamB));
            sb.AppendLine ($"{Ac.Divide} AB");
            sb.AppendLine ("xch A, 0F0h");
            return sb.ToString ();
        }
    }
}