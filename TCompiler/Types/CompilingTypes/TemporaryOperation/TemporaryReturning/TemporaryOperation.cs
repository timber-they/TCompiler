using TCompiler.Settings;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.TemporaryOperation.TemporaryReturning      //todo code documenation
{
    public class TemporaryOperation : ITemporaryReturning
    {
        public TemporaryOperation()
        {
            Sign = "";
            A = null;
            B = null;
        }

        public string Sign { get; set; }
        public ITemporaryReturning A { get; set; }
        public ITemporaryReturning B { get; set; }

        public ReturningCommand.ReturningCommand GetReturningCommand()
        {
            switch (Sign)
            {
                case "&":
                    return new And(A.GetReturningCommand(), B.GetReturningCommand());
                case "|":
                    return new Or(A.GetReturningCommand(), B.GetReturningCommand());
                case "!":
                    return new Not(B.GetReturningCommand());
                case "+":
                    return new Add(A.GetReturningCommand(), B.GetReturningCommand());
                case "-":
                    return new Subtract(A.GetReturningCommand(), B.GetReturningCommand());
                case "*":
                    return new Multiply(A.GetReturningCommand(), B.GetReturningCommand());
                case "/":
                    return new Divide(A.GetReturningCommand(), B.GetReturningCommand());
                case "%":
                    return new Modulo(A.GetReturningCommand(), B.GetReturningCommand());
                case ">":
                    return new Bigger(A.GetReturningCommand(), B.GetReturningCommand());
                case "<":
                    return new Smaller(A.GetReturningCommand(), B.GetReturningCommand());
                case "=":
                    return new Equal(A.GetReturningCommand(), B.GetReturningCommand());
                case "!=":
                    return new UnEqual(A.GetReturningCommand(), B.GetReturningCommand());
                case "++":
                {
                    var rc = A.GetReturningCommand();
                    if (!(rc is ByteVariableCall))
                        throw new ParameterException(GlobalProperties.LineIndex, rc.ToString());
                    return new Increment((ByteVariableCall) rc);
                }
                case "--":
                {
                    var rc = A.GetReturningCommand();
                    if (!(rc is ByteVariableCall))
                        throw new ParameterException(GlobalProperties.LineIndex, rc.ToString());
                    return new Decrement((ByteVariableCall) rc);
                }
                case "<<":
                    return new ShiftLeft(A.GetReturningCommand(), B.GetReturningCommand(), GlobalProperties.CurrentRegister, GlobalProperties.Label);
                case ">>":
                    return new ShiftRight(A.GetReturningCommand(), B.GetReturningCommand(), GlobalProperties.CurrentRegister, GlobalProperties.Label);
                case ".":
                    var bo = new BitOf(A.GetReturningCommand(), B.GetReturningCommand(), GlobalProperties.Label,
                        GlobalProperties.Label, GlobalProperties.Label, GlobalProperties.Label, GlobalProperties.CurrentRegister);
                    GlobalProperties.CurrentRegisterAddress--;
                    return bo;
                case ":":
                    var collection = (A.GetReturningCommand() as VariableCall)?.Variable as Collection;
                    if (collection == null)
                        throw new ParameterException(GlobalProperties.LineIndex,
                            (A as TemporaryVariableConstantMethodCallOrNothing)?.Value ?? ((TemporaryOperation) A).Sign);
                    return new VariableOfCollection(collection, B.GetReturningCommand());
                default:
                    throw new ParameterException(GlobalProperties.LineIndex, Sign);
            }
        }
    }
}