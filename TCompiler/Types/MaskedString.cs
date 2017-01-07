#region

using System.Linq;

#endregion

namespace TCompiler.Types
{
    /// <summary>
    /// A masked string. Doesn't fit right here because it's not even used in this project, but maybe it'll be in the future.
    /// </summary>
    public class MaskedString
    {
        /// <summary>
        /// Initializes a new MaskedString
        /// </summary>
        /// <returns>Nothing</returns>
        /// <param name="s">The string</param>
        public MaskedString(string s)
        {
            String = s;
        }

        /// <summary>
        /// The string
        /// </summary>
        /// <value>The string</value>
        private string String { get; }

        /// <summary>
        /// Specifies wether the given object equals this
        /// </summary>
        /// <param name="obj">The other object</param>
        /// <returns>Wether they're equal</returns>
        public override bool Equals(object obj)
            =>
            (obj is MaskedString && Equals((MaskedString) obj)) ||
            (obj is string && Equals(new MaskedString((string) obj)));

        /// <summary>
        /// Specifies wether the given MaskedString equals this
        /// </summary>
        /// <remarks>
        /// The masked parts get ignored
        /// </remarks>
        /// <param name="other">The other masked string</param>
        /// <returns>Wether they're equal</returns>
        private bool Equals(MaskedString other)
            =>
            (String.Length == other.String.Length) &&
            !String.ToCharArray()
                .Where((t, i) => (t != '_') && (other.String[i] != '_') && (t != other.String[i]))
                .Any();

        /// <summary>
        /// The Hashcode
        /// </summary>
        /// <returns>The Hashcode</returns>
        public override int GetHashCode() => String?.GetHashCode() ?? 0;
    }
}