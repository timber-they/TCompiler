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
        ForTil,
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

        Variable,
        VariableCall,
        Bool,
        Char,
        Int,
        Cint,


        Condition,
        Label
    }
}