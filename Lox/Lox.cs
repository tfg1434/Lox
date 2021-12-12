namespace Lox; 

static class Lox {
    public static Maybe<Lst<LexError>> Run(string src) {
        Lexer lexer = new(src);
        (Lst<Token> tokens, Lst<LexError> errors) = lexer.Lex();
    
        foreach (Token token in tokens)
            Console.WriteLine(token);

        return errors.Count == 0 ? Nothing : Just(errors);
    }
    
    public static string Report(int line, string where, string message)
        => $"[line {line}] Error: {where}: {message}";
}