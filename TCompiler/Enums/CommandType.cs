namespace TCompiler.Enums
{
    public enum CommandType
    {
        VariableConstantMethodCallOrNothing,
        Command,


        Block,
        IfBlock,
        WhileBlock,
        ForTil,
        Break,


        ReturningCommand,

        Method,
        MethodCall,
        Return,

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