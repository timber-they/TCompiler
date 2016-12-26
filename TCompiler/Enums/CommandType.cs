namespace TCompiler.Enums
{
    public enum CommandType
    {
        VariableConstantMethodCallOrNothing,
        Command,


        Block,
        EndBlock,
        IfBlock,
        EndIf,
        WhileBlock,
        EndWhile,
        ForTilBlock,
        EndForTil,
        Break,


        ReturningCommand,

        Method,
        MethodCall,
        Return,
        EndMethod,

        Operation,
        OneParameterOperation,
        TwoParameterOperation,
        And,
        Not,
        Or,
        Add,
        Subtract,
        Multiply,
        Divide,
        Modulo,
        Assignment,
        Increment,
        Decrement,
        ShiftLeft,
        ShiftRight,

        Variable,
        VariableCall,
        ByteVariableCall,
        BitVariableCall,
        Bool,
        Char,
        Int,
        Cint,

        Compare,
        Bigger,
        Smaller,
        Equal,
        UnEqual,


        Condition,
        Label,
        Sleep,

        Empty
    }
}