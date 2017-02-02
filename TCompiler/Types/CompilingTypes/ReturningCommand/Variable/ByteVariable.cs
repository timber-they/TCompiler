using System.Text;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    ///     The base class for every byteVariable like int
    /// </summary>
    public abstract class ByteVariable : Variable
    {
        /// <summary>
        ///     Initializes a new ByteVariable
        /// </summary>
        /// <param name="isConstant">Indicates wether the value is constant, so that it's saved in the value property</param>
        /// <param name="value">The value property where the value is saved when it's constant</param>
        /// <param name="address">The address of the variable when it isn't constant</param>
        /// <param name="name">The name of the variable to use in T when it isn't constant</param>
        protected ByteVariable(bool isConstant, byte value, Address address, string name)
            : base(address, name, isConstant)
        {
            Value = value;
        }

        /// <summary>
        ///     The value property where the value is saved when it's constant
        /// </summary>
        public byte Value { get; }

        /// <summary>
        /// Moves the Accumulator into this ByteVariable
        /// </summary>
        /// <returns>The assembler code to execute as a string</returns>
        public virtual string MoveAccuIntoThis()
        {
            if (!Address.IsInExtendedMemory)
                return $"mov {Address}, A";
            var sb = new StringBuilder();
            sb.AppendLine(Address.MoveThisIntoDataPointer());
            sb.AppendLine("movx @dptr, A");
            return sb.ToString();
        }

        /// <summary>
        /// Moves the specified variable into this ByteVariable
        /// </summary>
        /// <param name="variable">The other variable to take the value from</param>
        /// <returns>The assembler code to execute as a string</returns>
        public override string MoveVariableIntoThis(VariableCall variable)
            =>
                !Address.IsInExtendedMemory
                    ? $"mov {Address}, {(variable.Variable.IsConstant ? "#" + ((ByteVariable) variable.Variable).Value : Address.ToString())}"
                    : $"{variable}\n{MoveAccuIntoThis()}";

        /// <summary>
        /// Moves the value of this ByteVariable into the Accumulator
        /// </summary>
        /// <returns>The assembler code to execute as a string</returns>
        public string MoveThisIntoAccu()
        {
            if (IsConstant)
                return $"mov A, #{Value}";
            if (!Address.IsInExtendedMemory)
                return $"mov A, {Address}";
            var sb = new StringBuilder();
            sb.AppendLine(Address.MoveThisIntoDataPointer());
            sb.AppendLine("movx A, @dptr");
            return sb.ToString();
        }

        /// <summary>
        /// Moves the B-Register into this ByteVariable
        /// </summary>
        /// <returns>The assembler code to execute as a string</returns>
        public virtual string MoveBIntoThis()
        {
            if (!Address.IsInExtendedMemory)
                return $"mov {Address}, 0F0h";
            var sb = new StringBuilder();
            sb.AppendLine("mov A, 0F0h");
            sb.AppendLine(MoveAccuIntoThis());
            return sb.ToString();
        }

        /// <summary>
        /// Moves the variable of this ByteVariable into the B-Register
        /// </summary>
        /// <returns>The assembler code to execute as a string</returns>
        public string MoveThisIntoB()
        {
            if (!Address.IsInExtendedMemory)
                return $"mov 0F0h, {Address}";
            var sb = new StringBuilder();
            sb.AppendLine(MoveThisIntoAccu());
            sb.AppendLine("mov 0F0h, A");
            return sb.ToString();
        }
    }
}