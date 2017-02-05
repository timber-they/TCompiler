#region

using TCompiler.Settings;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

#endregion

namespace TCompiler.Types.CompilingTypes.TemporaryOperation.TemporaryReturning
{
    /// <summary>
    ///     Evaluates the temporar returning structure for an operation
    /// </summary>
    public class TemporaryOperation : ITemporaryReturning
    {
        /// <summary>
        ///     Initializes a new temporary operation that represents the temporar structure of an operation
        /// </summary>
        public TemporaryOperation()
        {
            Sign = "";
            A = null;
            B = null;
        }

        /// <summary>
        ///     The sign of the operation
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        ///     The first parameter of the operation
        /// </summary>
        public ITemporaryReturning A { get; set; }

        /// <summary>
        ///     The second parameter of the operation
        /// </summary>
        public ITemporaryReturning B { get; set; }

        /// <summary>
        ///     Evaluates the final returning command for this temporary operation
        /// </summary>
        /// <returns>The returning command as a ReturningCommand</returns>
        public ReturningCommand.ReturningCommand GetReturningCommand()
        {
            switch (Sign)
            {
                case "&":
                    return new And(A.GetReturningCommand(), B.GetReturningCommand(), GlobalProperties.CurrentLine);
                case "|":
                    return new Or(A.GetReturningCommand(), B.GetReturningCommand(), GlobalProperties.CurrentLine);
                case "!":
                    return new Not(B.GetReturningCommand(), GlobalProperties.CurrentLine);
                case "+":
                    return new Add(A.GetReturningCommand(), B.GetReturningCommand(), GlobalProperties.CurrentLine);
                case "-":
                    return new Subtract(A.GetReturningCommand(), B.GetReturningCommand(), GlobalProperties.CurrentLine);
                case "*":
                    return new Multiply(A.GetReturningCommand(), B.GetReturningCommand(), GlobalProperties.CurrentLine);
                case "/":
                    return new Divide(A.GetReturningCommand(), B.GetReturningCommand(), GlobalProperties.CurrentLine);
                case "%":
                    return new Modulo(A.GetReturningCommand(), B.GetReturningCommand(), GlobalProperties.CurrentLine);
                case ">":
                    return new Bigger(A.GetReturningCommand(), B.GetReturningCommand(), GlobalProperties.CurrentLine);
                case "<":
                    return new Smaller(A.GetReturningCommand(), B.GetReturningCommand(), GlobalProperties.CurrentLine);
                case "=":
                    return new Equal(A.GetReturningCommand(), B.GetReturningCommand(), GlobalProperties.CurrentLine);
                case "!=":
                    return new UnEqual(A.GetReturningCommand(), B.GetReturningCommand(), GlobalProperties.CurrentLine);
                case "++":
                {
                    var rc = A.GetReturningCommand();
                    if (!(rc is ByteVariableCall))
                        throw new ParameterException(GlobalProperties.CurrentLine, rc.ToString());
                    return new Increment((ByteVariableCall) rc, GlobalProperties.CurrentLine);
                }
                case "--":
                {
                    var rc = A.GetReturningCommand();
                    if (!(rc is ByteVariableCall))
                        throw new ParameterException(GlobalProperties.CurrentLine, rc.ToString());
                    return new Decrement((ByteVariableCall) rc, GlobalProperties.CurrentLine);
                }
                case "<<":
                    return new ShiftLeft(A.GetReturningCommand(), B.GetReturningCommand(),
                        GlobalProperties.CurrentRegister, GlobalProperties.Label, GlobalProperties.CurrentLine);
                case ">>":
                    return new ShiftRight(A.GetReturningCommand(), B.GetReturningCommand(),
                        GlobalProperties.CurrentRegister, GlobalProperties.Label, GlobalProperties.CurrentLine);
                case ".":
                    var bo = new BitOf(A.GetReturningCommand(), B.GetReturningCommand(), GlobalProperties.Label,
                        GlobalProperties.Label, GlobalProperties.Label, GlobalProperties.Label,
                        GlobalProperties.CurrentRegister, GlobalProperties.CurrentLine);
                    GlobalProperties.CurrentRegisterAddress--;
                    return bo;
                case ":":
                    var collection = (A.GetReturningCommand() as VariableCall)?.Variable as Collection;
                    if (collection == null)
                        throw new ParameterException(GlobalProperties.CurrentLine,
                            (A as TemporaryVariableConstantMethodCallOrNothing)?.Value ?? ((TemporaryOperation) A).Sign);
                    return new VariableOfCollection(collection, B.GetReturningCommand(), GlobalProperties.CurrentLine);
                default:
                    throw new ParameterException(GlobalProperties.CurrentLine, Sign);
            }
        }
    }
}