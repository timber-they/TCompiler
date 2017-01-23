namespace TCompiler.Types.CompilingTypes.ReturningCommand.Variable
{
    /// <summary>
    ///     A cint variable (signed integer)<br />
    ///     Syntax:<br />
    ///     cint i
    /// </summary>
    public class Cint : ByteVariable
    {
        /// <summary>
        ///     Initializes a new cint variable
        /// </summary>
        /// <param name="address">The address of the variable when it isn't constant</param>
        /// <param name="name">The name of the variable to use in T when it isn't constant</param>
        /// <param name="isConstant">Indicates wether the value is constant, so that it's saved in the value property</param>
        /// <param name="value">The value property where the value is saved when it's constant</param>
        public Cint(Address address, string name, bool isConstant, byte value = 0)
            : base(isConstant, value, address, name)
        {
        }
    }
}