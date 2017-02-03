#region

using System;

#endregion

namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    ///     Gets thrown when a pre-compile error occurs
    /// </summary>
    [Serializable]
    public class PreCompileErrorException : CompileException
    {
        /// <summary>
        ///     Initializes a new PreCompileErrorException
        /// </summary>
        /// <param name="error">The PreCompileError</param>
        public PreCompileErrorException(Error.Error error) : base(error.LineIndex, error.Message)
        {
        }
    }
}