namespace TCompiler.Types
{
    /// <summary>
    /// Is used for the bitOf Operation
    /// </summary>
    public class ConstantBitAddress
    {
        /// <summary>
        /// Specifies in which byte the bit is
        /// </summary>
        public int ByteAddress;
        /// <summary>
        /// Specifies which bit of the ByteAddress should be used
        /// </summary>
        public int BitOf;

        /// <summary>
        /// Initiates a new ConstantBitAddress
        /// </summary>
        /// <returns>Nothing</returns>
        /// <param name="byteAddress">The address of the byte in which the bit is</param>
        /// <param name="bitOf">The bit from the ByteAddress</param>
        public ConstantBitAddress(int byteAddress, int bitOf)
        {
            ByteAddress = byteAddress;
            BitOf = bitOf;
        }

        /// <summary>
        /// The stuff that can be written in assembler
        /// </summary>
        /// <returns>The stuff as a string</returns>
        public override string ToString() => $"{ByteAddress}.{BitOf}";
    }
}