#region

using TCompiler.Settings;
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
        public Assignment(Variable.Variable toAssign, ReturningCommand evaluation) : base(true, true)
        {
            ToAssign = toAssign;
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
        public override string ToString()
        {
            if (ToAssign is ByteVariable)
            {
                var call = Evaluation as ByteVariableCall;
                return call != null
                    ? ToAssign.MoveVariableIntoThis(call)
                    : $"{Evaluation}\n{((ByteVariable) ToAssign).MoveAccuIntoThis()}";
            }
            var variableOfCollectionVariable = ToAssign as VariableOfCollectionVariable;
            if (variableOfCollectionVariable != null)
                return $"{Evaluation}\n{variableOfCollectionVariable.MoveAccuIntoThis()}";

            var count = 0;
            var bitOfVariable = ToAssign as BitOfVariable;

            if (bitOfVariable != null)
            {
                bitOfVariable.RegisterLoop = GlobalProperties.CurrentRegister;
                count++;
            }

            var fin = $"{Evaluation}\n{((BitVariable) ToAssign).MoveAcc0IntoThis()}";
            GlobalProperties.CurrentRegisterAddress -= count;
            return fin;
        }
    }
}