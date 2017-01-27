namespace TCompiler.Enums
{
    /// <summary>
    ///     All the command types that exist as classes, same named as enums, for better enumeration
    /// </summary>
    public enum CommandType
    {
        VariableConstantMethodCallOperationOrNothing,
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
        InterruptServiceRoutine,
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
        VariableOfCollection,

        Variable,
        VariableCall,
        ByteVariableCall,
        BitVariableCall,
        Bool,
        Char,
        Int,
        Cint,
        Collection,
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