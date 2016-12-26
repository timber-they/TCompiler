using System;

namespace TCompiler.Types.CheckTypes.TCompileException
{
    public abstract class TCompileException : Exception
    {
        protected TCompileException(string message) : base(message)
        {
            
        }
    }
}