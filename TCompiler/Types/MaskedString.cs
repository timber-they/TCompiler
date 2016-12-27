using System;
using System.Collections;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace TCompiler.Types
{
    public class MaskedString
    {
        public MaskedString(string s)
        {
            String = s;
        }

        private string String { get; }

        public override bool Equals(object obj) => obj is MaskedString && Equals((MaskedString) obj) || obj is string && Equals(new MaskedString((string)obj));

        private bool Equals(MaskedString other)
            =>
            String.Length == other.String.Length &&
            !String.ToCharArray().Where((t, i) => t != '_' && other.String[i] != '_' && t != other.String[i]).Any();

        public override int GetHashCode() => String?.GetHashCode() ?? 0;
    }
}