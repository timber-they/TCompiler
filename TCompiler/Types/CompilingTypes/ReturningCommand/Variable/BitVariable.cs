#region

using System.Text;

using TCompiler.AssembleHelp;

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
        ///     Call this - it moves the bit of 0E0h.0 into this
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public virtual string MoveAcc0IntoThis()
        {
            if (!Address.IsInExtendedMemory)
                return $"{Ac.Move} C, 0E0h.0\nmov {Address}, C";
            var sb = new StringBuilder();
            sb.AppendLine(Address.MoveThisIntoDataPointer());
            sb.AppendLine($"{Ac.Move} C, 0E0h.0");
            sb.AppendLine($"{Ac.MoveExtended} A, @dptr");
            sb.AppendLine($"{Ac.Move} 0E0h.0, C");
            sb.AppendLine($"{Ac.MoveExtended} @dptr, A");
            return sb.ToString();
        }

        /// <summary>
        ///     Moves the specified variable into this BitVariable
        /// </summary>
        /// <param name="variable">The other variable to take the value from</param>
        /// <returns>The assembler code to execute as a string</returns>
        public override string MoveVariableIntoThis(VariableCall variable) => $"{variable}\n{MoveAcc0IntoThis()}";

        /// <summary>
        ///     Moves the value of this BitVariable into the first bit of the accu
        /// </summary>
        /// <returns>The assembler code to execute as a string</returns>
        public string MoveThisIntoAcc0()
        {
            if (IsConstant)
                return Value ? $"{Ac.SetBit} 0E0h.0" : $"{Ac.Clear} 0E0h.0";
            if (!Address.IsInExtendedMemory)
                return $"{Ac.Move} C, {Address}\nmov 0E0h.0, C";
            var sb = new StringBuilder();
            sb.AppendLine(Address.MoveThisIntoDataPointer());
            sb.AppendLine($"{Ac.MoveExtended} A, @dptr");
            sb.AppendLine($"{Ac.Move} C, 0E0h.{Address.BitOf}");
            sb.AppendLine($"{Ac.Move} 0E0h.0, C");
            return sb.ToString();
        }

        /// <summary>
        ///     Clears (sets it to false) the value of this BitVariable
        /// </summary>
        /// <returns>The assembler code to execute as a string</returns>
        public string Clear()
        {
            if (!Address.IsInExtendedMemory)
                return $"{Ac.Clear} {Address}";
            var sb = new StringBuilder();
            sb.AppendLine(Address.MoveThisIntoDataPointer());
            sb.AppendLine($"{Ac.MoveExtended} A, @dptr");
            sb.AppendLine($"{Ac.Clear} 0E0h.0");
            sb.AppendLine($"{Ac.MoveExtended} @dptr, A");
            return sb.ToString();
        }

        /// <summary>
        ///     Sets (sets it to true) the value of this BitVariable
        /// </summary>
        /// <returns>The assembler code to execute as a string</returns>
        public string Set()
        {
            if (!Address.IsInExtendedMemory)
                return $"{Ac.SetBit} {Address}";
            var sb = new StringBuilder();
            sb.AppendLine(Address.MoveThisIntoDataPointer());
            sb.AppendLine($"{Ac.MoveExtended} A, @dptr");
            sb.AppendLine($"{Ac.SetBit} 0E0h.0");
            sb.AppendLine($"{Ac.MoveExtended} @dptr, A");
            return sb.ToString();
        }
    }
}