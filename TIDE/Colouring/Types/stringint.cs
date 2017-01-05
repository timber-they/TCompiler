namespace TIDE.Colouring.Types
{
    public class Stringint
    {
        public int Theint { get; }
        public string Thestring { get; }

        public Stringint(string thestring, int theint)
        {
            Thestring = thestring;
            Theint = theint;
        }
    }
}