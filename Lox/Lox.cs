namespace Lox; 

public static class Lox {
    public static Maybe<Lst<LexError>> Run(string src) {
        Lexer lexer = new(src);
        (Lst<Token> tokens, Lst<LexError> errors) = lexer.Lex();
    
        foreach (Token token in tokens)
            Console.WriteLine(token);

        return errors.Count == 0 ? Nothing : Just(errors);
    }
}