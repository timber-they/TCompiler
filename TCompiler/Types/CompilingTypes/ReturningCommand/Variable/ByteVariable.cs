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

        public virtual string MoveAccuIntoThis()
        {
            if (!Address.IsInExtendedMemory)
                return $"mov {Address}, A";
            var sb = new StringBuilder();
            sb.AppendLine(Address.MoveThisIntoDataPointer());
            sb.AppendLine("movx @dptr, A");
            return sb.ToString();
        }

        public override string MoveVariableIntoThis(VariableCall variable)
            =>
                !Address.IsInExtendedMemory
                    ? $"mov {this}, {(variable.Variable.IsConstant ? "#" + ((ByteVariable) variable.Variable).Value : ToString())}"
                    : $"{variable}\n{MoveAccuIntoThis()}";

        public string MoveThisIntoAccu()
        {
            if (!Address.IsInExtendedMemory)
                return $"mov A, {ToString()}";
            var sb = new StringBuilder();
            sb.AppendLine(Address.MoveThisIntoDataPointer());
            sb.AppendLine("movx A, @dptr");
            return sb.ToString();
        }

        public virtual string MoveBIntoThis()
        {
            if (!Address.IsInExtendedMemory)
                return $"mov {ToString()}, 0F0h";
            var sb = new StringBuilder();
            sb.AppendLine("mov A, 0F0h");
            sb.AppendLine(MoveAccuIntoThis());
            return sb.ToString();
        }

        public string MoveThisIntoB()
        {
            if (!Address.IsInExtendedMemory)
                return $"mov 0F0h, {ToString()}";
            var sb = new StringBuilder();
            sb.AppendLine(MoveThisIntoAccu());
            sb.AppendLine("mov 0F0h, A");
            return sb.ToString();
        }
    }
}