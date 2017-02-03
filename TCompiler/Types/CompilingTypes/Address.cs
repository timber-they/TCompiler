namespace TCompiler.Types.CompilingTypes
{
    /// <summary>
    /// The address of a variable. Can be in the xMem
    /// </summary>
    public class Address
    {
        /// <summary>
        /// The Byte (Base) Address of the variable
        /// </summary>
        private int ByteAddress { get; }
        /// <summary>
        /// If neccessary the bit of the ByteAddress
        /// </summary>
        public int? BitOf { get; }
        /// <summary>
        /// Indicates wether the address is located in the external memory
        /// </summary>
        public bool IsInExtendedMemory { get; }
        /// <summary>
        /// Indicates wether the variable is a special function register variable
        /// </summary>
        /// <returns>The indicator as a boolean</returns>
        private bool IsInSpecialFunctionRegister() => ByteAddress >= 0x80 && !IsInExtendedMemory;
        /// <summary>
        /// Indicates wether the variable in the SFR is BitAddressable
        /// </summary>
        /// <returns>The indicator as a boolean</returns>
        public bool IsBitAddressableInSpecialFunctionRegister() => !IsInSpecialFunctionRegister() && ByteAddress % 8 == 0;

        /// <summary>
        /// Initializes a new Address
        /// </summary>
        /// <param name="byteAddress">The Byte (Base) Address of the variable</param>
        /// <param name="isInExtendedMemory">Indicates wether the address is located in the external memory</param>
        /// <param name="bitOf">If neccessary the bit of the ByteVariable</param>
        public Address(int byteAddress, bool isInExtendedMemory, int? bitOf = null)
        {
            BitOf = bitOf;
            ByteAddress = byteAddress;
            IsInExtendedMemory = isInExtendedMemory;
        }

        /// <summary>
        /// Think very carefully about using this!<br />
        /// Keep in mind that you can't use it with extendedMemory variables!
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{ByteAddress}{(BitOf == null ? "" : $".{BitOf}")}";

        /// <summary>
        /// Evaluates the following address of this. If neccessary it locates the new address into the xMem.
        /// </summary>
        public Address NextAddress => BitOf != null
            ? (BitOf < 7
                ? new Address(ByteAddress, IsInExtendedMemory, BitOf + 1)
                : (ByteAddress >= 0x2F && !IsInExtendedMemory ? new Address(0, true, 0) : new Address(ByteAddress + 1, IsInExtendedMemory, 0)))
            : (ByteAddress >= 0x80 && !IsInExtendedMemory ? new Address(0, true) : new Address(ByteAddress + 1, IsInExtendedMemory));

        /// <summary>
        /// Evaluates the previous address of this. If neccessary it jumps back to the normal RAM.
        /// </summary>
        public Address PreviousAddress => BitOf != null
            ? (BitOf > 0
                ? new Address(ByteAddress, IsInExtendedMemory, BitOf - 1)
                : (IsInExtendedMemory && ByteAddress == 0 ? new Address(0x79, false, 7) : new Address(ByteAddress - 1, IsInExtendedMemory, 7)))
            : (IsInExtendedMemory && ByteAddress == 0 ? new Address(0x79, false) : new Address(ByteAddress - 1, IsInExtendedMemory));

        /// <summary>
        /// Moves this address into the DataPointer (dptr), to access it if it's inside of the xMem
        /// </summary>
        /// <returns>The assembler code to execute as a string</returns>
        public string MoveThisIntoDataPointer() => $"mov dptr, {ByteAddress}";
    }
}