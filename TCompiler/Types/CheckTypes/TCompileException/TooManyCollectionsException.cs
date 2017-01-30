namespace TCompiler.Types.CheckTypes.TCompileException
{
    public class TooManyCollectionsException : TooManyException
    {
        public TooManyCollectionsException(int line, string message="Actually the collection can't be in the extended memory yet!") : base(line, message)
        {
        }
    }
}