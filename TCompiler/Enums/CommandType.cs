// ReSharper disable UnusedMember.Global
namespace TCompiler.Enums
{
    public enum CommandType
    {
        VariableConstantMethodCallOrNothing,
        Command,


        Block,
        EndBlock,
        IfBlock,
        ElseBlock,
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
        AddAssignment,
        AndAssignment,
        DivideAssignment,
        ModuloAssignment,
        MultiplyAssignment,
        OrAssignment,
        SubtractAssignment,

        Increment,
        Decrement,
        ShiftLeft,
        ShiftRight,
        BitOf,

        Variable,
        VariableCall,
        ByteVariableCall,
        BitVariableCall,
        Bool,
        Char,
        Int,
        Cint,
        Declaration,

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