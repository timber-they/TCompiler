namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class ElseWithoutIfException : CompileException
    {
        public ElseWithoutIfException(int line, string message="Else cannot stand alone") : base(line, message)
        {
        }
    }
}