using System;

namespace TCompiler.Types.CheckTypes.TCompileException
{
    public abstract class TCompileException : Exception
    {
        protected TCompileException(int line, string message) : base(message)
        {
            Line = line;
        }

        public int Line { get; }
    }
}