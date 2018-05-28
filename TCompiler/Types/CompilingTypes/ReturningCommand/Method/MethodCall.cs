#region

using System.Collections.Generic;
using System.Text;

using TCompiler.Types.CompilerTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion


namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    /// <summary>
    ///     The command to call a method<br />
    ///     Syntax:<br />
    ///     methodName [param1, param2]
    /// </summary>
    public class MethodCall : ReturningCommand
    {
        /// <summary>
        ///     Initiates a new methodCall
        /// </summary>
        /// <param name="method">The method that is called</param>
        /// <param name="parameterValues">The values (as variable calls) for the parameters in the called method</param>
        /// <param name="cLine">The original T code line</param>
        public MethodCall (Method method, List <ReturningCommand> parameterValues, CodeLine cLine) : base (true, true,
                                                                                                           cLine)
        {
            Method          = method;
            ParameterValues = parameterValues;
        }

        /// <summary>
        ///     The method that is called
        /// </summary>
        private Method Method { get; }

        /// <summary>
        ///     The values (as variable calls) for the parameters in the called method
        /// </summary>
        private List <ReturningCommand> ParameterValues { get; }

        /// <summary>
        ///     Parses the MethodCall command object to assembler code
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString ()
        {
            var sb = new StringBuilder ();
            for (var i = 0; i < ParameterValues.Count; i++)
                sb.AppendLine (ParameterValues [i] +
                               "\n" +
                               (Method.Parameters [i] is ByteVariable
                                    ? ((ByteVariable) Method.Parameters [i]).MoveAccuIntoThis ()
                                    : ((BitVariable) Method.Parameters [i]).MoveAcc0IntoThis ()));
            return $"{sb}\ncall {Method.Label.DestinationName}";
        }
    }
}