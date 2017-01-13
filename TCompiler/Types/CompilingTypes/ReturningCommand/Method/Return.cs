using System.Linq;

namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    /// <summary>
    /// The command to return (a value) from a method
    /// </summary>
    public class Return : ReturningCommand
    {
        /// <summary>
        /// The value that is being returned
        /// </summary>
        /// <remarks>Can be null</remarks>
        private readonly ReturningCommand _toReturn;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toReturn"></param>
        public Return(ReturningCommand toReturn) : base(true, false, toReturn.ExpectedSplitterLengths?.Select(i => i+1))
        {
            _toReturn = toReturn;
        }


        public override string ToString() => _toReturn != null ? $"{_toReturn}\nret" : "ret";
    }
}