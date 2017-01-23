namespace TCompiler.Types.CompilingTypes
{
    public class Address
    {
        private int ByteAddress { get; }
        private int? BitOf { get; }

        public Address(int byteAddress, int? bitOf = null)
        {
            BitOf = bitOf;
            ByteAddress = byteAddress;
        }

        public override string ToString() => $"{ByteAddress}{(BitOf == null ? "" : $".{BitOf}")}";

        public Address NextAddress => BitOf != null
            ? (BitOf < 7
                ? new Address(ByteAddress, BitOf + 1)
                : (ByteAddress >= 0x2F ? null : new Address(ByteAddress + 1, 0)))
            : (ByteAddress >= 0x80 ? null : new Address(ByteAddress + 1));

        public Address PreviousAddress => BitOf != null
            ? (BitOf > 0
                ? new Address(ByteAddress, BitOf - 1)
                : new Address(ByteAddress - 1, 7))
            : new Address(ByteAddress - 1);
    }
}