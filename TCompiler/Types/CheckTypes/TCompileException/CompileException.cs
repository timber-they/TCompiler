using System;

namespace TCompiler.Types.CheckTypes.TCompileException
{
    public abstract class CompileException : Exception
    {
        protected CompileException(int line, string message) : base(message)
        {
            Line = line;
        }

        public int Line { get; }
    }
}