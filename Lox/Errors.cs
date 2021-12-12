namespace Lox; 

public record class QuitReplError() : Error("User quit the REPL (line was null)");

public record class LexError : Error {
    public int Line { get; init; }
    
    public LexError(int line, string message) : base(message) 
        => Line = line;
}

