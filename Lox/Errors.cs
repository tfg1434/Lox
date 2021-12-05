namespace Lox; 

public record class QuitReplError() : Error("User quit the REPL (line was null)");

public record class CodeError : Error {
    public int Line { get; init; }
    
    public CodeError(int line, string message) : base(message) 
        => Line = line;
}

