namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    /// The base class for every byteVariable like int
    /// </summary>
    public abstract class ByteVariable : Variable
    {
        /// <summary>
        /// Initializes a new ByteVariable
        /// </summary>
        /// <param name="isConstant">Indicates wether the value is constant, so that it's saved in the value property</param>
        /// <param name="value">The value property where the value is saved when it's constant</param>
        /// <param name="address">The address of the variable when it isn't constant</param>
        /// <param name="name">The name of the variable to use in T when it isn't constant</param>
        protected ByteVariable(bool isConstant, byte value, string address, string name)
            : base(address, name, isConstant)
        {
            Value = value;
        }

        /// <summary>
        /// The value property where the value is saved when it's constant
        /// </summary>
        public byte Value { get; }

        /// <summary>
        /// Call this to get the value of the variable in assembler
        /// </summary>
        /// <returns>The assembler code as a string</returns>
        public override string ToString() => IsConstant ? $"#{Value}" : base.ToString();
    }
}