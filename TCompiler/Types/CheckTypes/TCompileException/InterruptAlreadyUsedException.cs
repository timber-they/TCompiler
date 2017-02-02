#region

using System;
using TCompiler.Enums;

#endregion

namespace TCompiler.Types.CheckTypes.TCompileException
{
    [Serializable]
    public class InterruptAlreadyUsedException : CompileException
    {
        public InterruptAlreadyUsedException(int lineIndex, InterruptType type,
            string message = "The interrupt type {0} is already used!") : base(lineIndex, string.Format(message, type))
        {
        }
    }
}