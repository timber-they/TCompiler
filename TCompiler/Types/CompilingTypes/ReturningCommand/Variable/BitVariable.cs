#region

using System;

#endregion

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    ///     The base class for bit variables (currently only bool)
    /// </summary>
    public abstract class BitVariable : Variable
    {
        /// <summary>
        ///     Initializes a new BitVariable
        /// </summary>
        /// <param name="isConstant">Indicates wether the value is constant, so that it's saved in the value property</param>
        /// <param name="value">The value property where the value is saved when it's constant</param>
        /// <param name="address">The address of the variable when it isn't constant</param>
        /// <param name="name">The name of the variable to use in T when it isn't constant</param>
        protected BitVariable(bool isConstant, bool value, Address address, string name)
            : base(address, name, isConstant)
        {
            Value = value;
        }

        /// <summary>
        ///     The value property where the value is saved when it's constant
        /// </summary>
        public bool Value { get; }

        /// <summary>
        ///     Don't call this - it just throws an exception
        /// </summary>
        /// <returns>Nothing</returns>
        public override string ToString()
        {
            throw new Exception("Use move bit to");
            //return IsConstant ? $"#{(Value ? "1" : "0")}" : base.ToString();
        }

        /// <summary>
        ///     Call this - it moves the bit of 224.0 into this
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public virtual string MoveAcc0IntoThis() => $"mov C, 224.0\nmov {Address}, C";
    }
}