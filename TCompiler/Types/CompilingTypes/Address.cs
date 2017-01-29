namespace TCompiler.Types.CompilingTypes
{
    public class Address
    {
        private int ByteAddress { get; }
        public int? BitOf { get; }
        public bool IsInExtendedMemory { get; }
        private bool IsInSpecialFunctionRegister() => ByteAddress >= 0x80 && !IsInExtendedMemory;
        public bool IsBitAddressableInSpecialFunctionRegister() => IsInSpecialFunctionRegister() && ByteAddress % 8 == 0;

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

        public Address NextAddress => BitOf != null
            ? (BitOf < 7
                ? new Address(ByteAddress, IsInExtendedMemory, BitOf + 1)
                : (ByteAddress >= 0x2F ? new Address(0, true, 0) : new Address(ByteAddress + 1, IsInExtendedMemory, 0)))
            : (ByteAddress >= 0x80 ? new Address(0, true) : new Address(ByteAddress + 1, IsInExtendedMemory));

        public Address PreviousAddress => BitOf != null
            ? (BitOf > 0
                ? new Address(ByteAddress, IsInExtendedMemory, BitOf - 1)
                : (IsInExtendedMemory && ByteAddress == 0 ? new Address(0x79, false, 7) : new Address(ByteAddress - 1, IsInExtendedMemory, 7)))
            : (IsInExtendedMemory && ByteAddress == 0 ? new Address(0x79, false) : new Address(ByteAddress - 1, IsInExtendedMemory));

        public string MoveThisIntoDataPointer() => $"mov dptr, {ByteAddress}";
    }
}