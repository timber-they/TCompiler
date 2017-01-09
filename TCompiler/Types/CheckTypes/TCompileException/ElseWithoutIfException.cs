﻿namespace TCompiler.Types.CheckTypes.TCompileException
{
    /// <summary>
    /// Gets thrown when the user has an else without an if around it
    /// </summary>
    public class ElseWithoutIfException : CompileException
    {
        /// <summary>
        /// Initializes a new ElseWithoutIfException
        /// </summary>
        /// <param name="line">The line the exception got thrown</param>
        /// <param name="message">The message to show to the user</param>
        public ElseWithoutIfException(int line, string message="Else cannot stand alone") : base(line, message)
        {
        }
    }
}