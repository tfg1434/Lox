namespace Lox; 

public static class Lox {
    public static Maybe<Lst<CodeError>> Run(string src) {
        Lexer lexer = new(src);
        (Lst<Token> tokens, Lst<CodeError> errors) = lexer.Lex();
    
        foreach (Token token in tokens)
            Console.WriteLine(token);

        return errors.Count == 0 ? Just(errors) : Nothing;
    }
}