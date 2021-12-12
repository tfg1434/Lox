namespace Lox; 

record class QuitReplError() : Error("User quit the REPL (line was null)");

record class LexError : Error {
    public int Line { get; init; }
    
    public LexError(int line, string message) : base(message) 
        => Line = line;
}

record class ParseError : Error {
    public Token Token { get; init; }
    public int Line { get; init; }

    public ParseError(Token token, int line, string message) : base(message) {
        Token = token;
        Line = line;
    }
}

