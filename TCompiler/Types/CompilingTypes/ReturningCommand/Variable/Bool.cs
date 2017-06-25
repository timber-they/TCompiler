namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    ///     A boolean variable<br />
    ///     Syntax:<br />
    ///     bool name
    /// </summary>
    public class Bool : BitVariable
    {
        /// <summary>
        ///     Initializes a new boolean variable
        /// </summary>
        /// <param name="address">The address of the variable when it isn't constant</param>
        /// <param name="name">The name of the variable to use in T when it isn't constant</param>
        /// <param name="isConstant">Indicates wether the value is constant, so that it's saved in the value property</param>
        /// <param name="value">The value property where the value is saved when it's constant</param>
        public Bool (Address address, string name, bool isConstant, bool value = false)
            : base (isConstant, value, address, name) {}
    }
}