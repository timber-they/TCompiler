#region

using System.Text;

using TCompiler.AssembleHelp;
using TCompiler.Types.CompilerTypes;

#endregion


namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     Divides paramB from paramA<br />
    ///     Syntax:<br />
    ///     paramA / paramB
    /// </summary>
    public class Divide : TwoParameterOperation
    {
        /// <summary>
        ///     Initiates a new divide operation
        /// </summary>
        /// <param name="paramA">The first parameter to divide from</param>
        /// <param name="paramB">The second parameter that divides the first one</param>
        /// <param name="cLine">The original T code line</param>
        public Divide (ReturningCommand paramA, ReturningCommand paramB, CodeLine cLine) :
            base (paramA, paramB, cLine) {}

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a divide operation
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString ()
        {
            var sb = new StringBuilder ();
            sb.AppendLine (AssembleCodePreviews.MoveParametersIntoAb (ParamA, ParamB));
            sb.AppendLine ($"{Ac.Divide} AB");
            return sb.ToString ();
        }
    }
}