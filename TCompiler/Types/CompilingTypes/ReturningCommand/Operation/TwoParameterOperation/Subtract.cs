#region

using System.Text;

using TCompiler.AssembleHelp;
using TCompiler.Types.CompilerTypes;

#endregion


namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    /// <summary>
    ///     Subtracts paramB from paramA<br />
    ///     Syntax:<br />
    ///     paramA - paramB
    /// </summary>
    public class Subtract : TwoParameterOperation
    {
        /// <summary>
        ///     Initiates a new Subtract operation
        /// </summary>
        /// <param name="paramA">The parameter to subtract paramB from</param>
        /// <param name="paramB">The parameter that is being subtracted from paramA</param>
        /// <param name="cLine">The original T code line</param>
        public Subtract (ReturningCommand paramA, ReturningCommand paramB, CodeLine cLine) :
            base (paramA, paramB, cLine) {}

        /// <summary>
        ///     Evaluates the stuff to execute in assembler to make a Subtract operation
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString ()
        {
            var sb = new StringBuilder ();
            sb.AppendLine (AssembleCodePreviews.MoveParametersIntoAb (ParamA, ParamB));
            sb.AppendLine ($"{Ac.Clear} C");
            sb.AppendLine ($"{Ac.Subtract} A, 0F0h");
            return sb.ToString ();
        }
    }
}