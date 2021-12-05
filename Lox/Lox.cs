namespace Lox; 

public static class Lox {
    public static Maybe<Lst<CodeError>> Run(string src) {
        Lexer lexer = new Lexer(src);
        Lst<Token> tokens = lexer.Lex();
    
        foreach (Token token in tokens)
            Console.WriteLine(token);
    }
}