using System;
using TCompiler.Types.CompilingTypes.ReturningCommand.Variable;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation
{
    public class BitOf : TwoParameterOperation
    {
        private readonly Label _l1;
        private readonly Label _lend;

        public BitOf(VariableCall paramA, VariableCall paramB, Label lend, Label l1) : base(paramA, paramB)
        {
            _lend = lend;
            _l1 = l1;
        }

        public BitOf(Tuple<VariableCall, VariableCall> pars, Label lend, Label l1) : base(pars)
        {
            _lend = lend;
            _l1 = l1;
        }

        public override string ToString()
            =>
            $"{_paramA}\njb acc.{((ByteVariableCall) _paramB).Variable.Value}\nclr acc.0\njmp {_lend}\n{_l1}:\nsetb acc.0\n{_lend}:";
    }
}