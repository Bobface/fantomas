﻿namespace Fantomas

open Fantomas.FormatConfig
open Microsoft.FSharp.Compiler.Ast
open Microsoft.FSharp.Compiler.Range
open Microsoft.FSharp.Compiler.SourceCodeServices

[<Sealed>]
type CodeFormatter =
    static member Parse : fileName:string * source:string * projectOptions:FSharpProjectOptions * checker:FSharpChecker -> Async<ParsedInput> 
    static member FormatAST : ast:ParsedInput * source:string option * config:FormatConfig -> string
    
    static member FormatAroundCursor : 
        fileName:string * cursorPos:pos * source:string * config:FormatConfig * projectOptions:FSharpProjectOptions * checker:FSharpChecker -> Async<string>
    
    static member FormatDocument : 
        fileName:string * source:string * config:FormatConfig * projectOptions:FSharpProjectOptions * checker:FSharpChecker -> Async<string>
    
    static member FormatSelection : 
        fileName:string * selection:range * source:string * config:FormatConfig * projectOptions:FSharpProjectOptions * checker:FSharpChecker -> Async<string>
    
    static member IsValidAST : ast:ParsedInput -> bool
    static member IsValidFSharpCode : fileName:string * source:string * projectOptions:FSharpProjectOptions * checker:FSharpChecker -> Async<bool>
    
    static member MakePos : line:int * col:int -> pos
    static member MakeRange : startLine:int * startCol:int * endLine:int * endCol:int -> range

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module CodeFormatter =
    /// Parse a source code string
    val parse : isFsiFile:bool -> sourceCode:string -> ParsedInput

    val makePos : line:int -> col:int -> pos
    val makeRange : startLine:int -> startCol:int -> endLine:int -> endCol:int -> range

    /// Check whether an AST consists of parsing errors 
    val isValidAST : ast:ParsedInput -> bool

    /// Check whether an input string is invalid in F# by looking for erroneous nodes in ASTs
    val isValidFSharpCode : isFsiFile:bool -> sourceCode:string -> bool

    /// Format a source string using given config
    val formatSourceString : isFsiFile:bool -> sourceCode:string -> config:FormatConfig -> string

    /// Format an abstract syntax tree using given config
    val formatAST : ast:ParsedInput -> sourceCode:string option -> config:FormatConfig -> string

    /// Format a part of source string using given config, and return the (formatted) selected part only.
    /// Beware that the range argument is inclusive. If the range has a trailing newline, it will appear in the formatted result.
    val formatSelectionOnly : isFsiFile:bool -> range:range -> sourceCode:string -> config:FormatConfig -> string

    /// Format a selected part of source string using given config; expanded selected ranges to parsable ranges. 
    val formatSelectionExpanded : isFsiFile:bool -> range:range -> sourceCode:string -> config:FormatConfig -> string * range

    /// Format a selected part of source string using given config; keep other parts unchanged. 
    val formatSelectionFromString : isFsiFile:bool -> range:range -> sourceCode:string -> config:FormatConfig -> string

    /// Format around cursor delimited by '[' and ']', '{' and '}' or '(' and ')' using given config; keep other parts unchanged. 
    val formatAroundCursor : isFsiFile:bool -> cursorPos:pos -> sourceCode:string -> config:FormatConfig -> string