#region

using System;

using TCompiler.Enums;
using TCompiler.Types.CompilerTypes;

#endregion


namespace TCompiler.Types.CheckTypes.TCompileException
{
    [Serializable]
    public class InterruptAlreadyUsedException : CompileException
    {
        public InterruptAlreadyUsedException (
            CodeLine codeLineIndex, InterruptType type,
            string   message = "The interrupt type {0} is already used!") : base (codeLineIndex,
                                                                                  string.Format (message, type)) {}
    }
}