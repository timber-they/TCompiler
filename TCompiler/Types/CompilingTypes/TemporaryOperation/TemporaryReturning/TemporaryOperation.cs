using TCompiler.Compiling;
using TCompiler.Types.CheckTypes.TCompileException;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.OneParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation;
using TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.Compare;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.TemporaryOperation.TemporaryReturning
{
    public class TemporaryOperation : ITemporaryReturning
    {
        public TemporaryOperation(string sign, ITemporaryReturning a, ITemporaryReturning b)
        {
            Sign = sign;
            A = a;
            B = b;
        }

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
                    return new Increment(A.GetReturningCommand());
                case "--":
                    return new Decrement(A.GetReturningCommand());
                case "<<":
                    return new ShiftLeft(A.GetReturningCommand(), B.GetReturningCommand(), ParseToObjects.CurrentRegister, ParseToAssembler.Label);
                case ">>":
                    return new ShiftRight(A.GetReturningCommand(), B.GetReturningCommand(), ParseToObjects.CurrentRegister, ParseToAssembler.Label);
                case ".":
                    var bo = new BitOf(A.GetReturningCommand(), B.GetReturningCommand(), ParseToAssembler.Label,
                        ParseToAssembler.Label, ParseToAssembler.Label, ParseToObjects.CurrentRegister);
                    ParseToObjects.CurrentRegisterAddress--;
                    return bo;
                case ":":
                    var collection = (A.GetReturningCommand() as VariableCall)?.Variable as Collection;
                    if (collection == null)
                        throw new ParameterException(ParseToObjects.LineIndex,
                            (A as TemporaryVariableConstantMethodCallOrNothing)?.Value ?? ((TemporaryOperation) A).Sign);
                    return new VariableOfCollection(collection, B.GetReturningCommand());
                default:
                    throw new ParameterException(ParseToObjects.LineIndex, Sign);
            }
        }
    }
}