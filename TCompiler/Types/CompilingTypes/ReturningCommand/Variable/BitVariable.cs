using System;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    public abstract class BitVariable : Variable
    {
        protected BitVariable(bool isConstant, bool value, string address, string name)
            : base(isConstant, address, name)
        {
            Value = value;
        }

        public bool Value { get; }

        public override string ToString()
        {
            throw new Exception("Use move bit to");
            //return IsConstant ? $"#{(Value ? "1" : "0")}" : base.ToString();
        }

        public virtual string MoveAcc0IntoThis() => $"mov C, acc.0\nmov {Address}, C";
    }
}