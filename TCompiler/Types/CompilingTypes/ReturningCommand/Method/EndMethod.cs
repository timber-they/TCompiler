﻿namespace TCompiler.Types.CompilingTypes.ReturningCommand.Method
{
    /// <summary>
    /// Specifies the end of the current method<br/>
    /// Syntax<br/>
    /// endmethod
    /// </summary>
    public class EndMethod : Command
    {
        /// <summary>
        /// Initiates a new endMethod command
        /// </summary>
        public EndMethod() : base (false, false, new []{1})
        {
            
        }
    }
}