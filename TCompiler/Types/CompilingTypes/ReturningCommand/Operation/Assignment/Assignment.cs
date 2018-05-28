#region

using TCompiler.Settings;
using TCompiler.Types.CompilerTypes;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion


namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment
{
    /// <summary>
    ///     A normal assignment and the base class for special assignments<br />
    ///     Syntax:<br />
    ///     toAssign := evaluation
    /// </summary>
    public class Assignment : Operation
    {
        /// <summary>
        ///     Initiates a new assignment
        /// </summary>
        /// <param name="toAssign">The variable to assign the result to</param>
        /// <param name="evaluation">The stuff to execute before the value of A (or 0E0h.0) is written into the toAssign variable</param>
        /// <param name="cLine">The original T code line</param>
        public Assignment (Variable.Variable toAssign, ReturningCommand evaluation, CodeLine cLine) : base (true, true,
                                                                                                            cLine)
        {
            ToAssign   = toAssign;
            Evaluation = evaluation;
        }

        /// <summary>
        ///     The stuff to execute before the value of A (or 0E0h.0) is written into the toAssign variable
        /// </summary>
        protected ReturningCommand Evaluation { get; }

        /// <summary>
        ///     The variable to assign the result to
        /// </summary>
        protected Variable.Variable ToAssign { get; }

        /// <summary>
        ///     Evaluates the stuff to execute in assembler for an assignment
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString ()
        {
            var byteVariable = ToAssign as ByteVariable;
            if (byteVariable != null)
            {
                var call = Evaluation as ByteVariableCall;
                return call != null
                           ? ToAssign.MoveVariableIntoThis (call)
                           : $"{Evaluation}\n{byteVariable.MoveAccuIntoThis ()}";
            }

            var variableOfCollectionVariable = ToAssign as VariableOfCollectionVariable;
            if (variableOfCollectionVariable != null)
                return $"{Evaluation}\n{variableOfCollectionVariable.MoveAccuIntoThis ()}";
            var bitOfVariable = ToAssign as BitOfVariable;

            if (bitOfVariable == null)
                return $"{Evaluation}\n{((BitVariable) ToAssign).MoveAcc0IntoThis ()}";

            bitOfVariable.RegisterLoop = GlobalProperties.CurrentRegister;
            var fin = $"{Evaluation}\n{((BitOfVariable) ToAssign).MoveAcc0IntoThis ()}";
            GlobalProperties.CurrentRegisterAddress--;
            return fin;
        }
    }
}