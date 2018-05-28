// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;


[assembly:
    SuppressMessage ("Style", "IDE0019:Use pattern matching",
        Justification = "<Pending>", Scope = "member",
        Target =
            "~M:TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment.AndAssignment.ToString~System.String")]
[assembly:
    SuppressMessage ("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly",
        Scope = "type", Target = "TCompiler.Types.CheckTypes.TCompileException.CompileException")]
[assembly:
    SuppressMessage ("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable",
        Scope = "type", Target = "TCompiler.Types.CheckTypes.TCompileException.FileDoesntExistException")]
[assembly:
    SuppressMessage ("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly",
        Scope = "type", Target = "TCompiler.Types.CheckTypes.TCompileException.InvalidCommandException")]
[assembly:
    SuppressMessage ("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly",
        Scope = "type", Target = "TCompiler.Types.CheckTypes.TCompileException.InvalidSleepTimeException")]
[assembly:
    SuppressMessage ("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable",
        Scope = "type", Target = "TCompiler.Types.CheckTypes.TCompileException.MethodExistsException")]
[assembly:
    SuppressMessage ("Microsoft.Design",
        "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Scope = "member",
        Target =
            "TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment.AndAssignment.#ToString()")]
[assembly:
    SuppressMessage ("Microsoft.Design",
        "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Scope = "member",
        Target =
            "TCompiler.Types.CompilingTypes.ReturningCommand.Operation.Assignment.OrAssignment.#ToString()")]
[assembly:
    SuppressMessage ("Microsoft.Design",
        "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Scope = "member",
        Target =
            "TCompiler.Types.CompilingTypes.ReturningCommand.Operation.TwoParameterOperation.BitOf.#ToString()")]
[assembly:
    SuppressMessage ("Microsoft.Design",
        "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Scope = "member",
        Target =
            "TCompiler.Types.CompilingTypes.ReturningCommand.Variable.Variable.#ToString()")]
[assembly:
    SuppressMessage ("Style", "IDE0018:Inline variable declaration",
        Justification = "<Pending>", Scope = "member",
        Target =
            "~M:TCompiler.Compiling.ParseToObjects.ParseTCodeToCommands(System.Collections.Generic.IEnumerable{System.Collections.Generic.List{TCompiler.Types.CompilerTypes.CodeLine}})~System.Collections.Generic.IEnumerable{TCompiler.Types.CompilingTypes.Command}")]