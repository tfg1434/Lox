using static Lox.TokenType;

namespace Lox; 

class Lexer {
    private readonly string _src;
    
    public Lexer(string src)
        => _src = src;

    private ((string Str, int Line) State, Lst<Token> Tokens) LexRec((string Str, int Line) state, Lst<Token> tokens) {
        // Lst<Token> AddToken(Lst<Token> tokens, TokenType type, string text, Maybe<OneOf<string, double>> literal)
        //     => tokens.Append(new Token(type, text, literal, ));

        int start = 0;
        int end = 0;
        string src = state.Str;
        int line = state.Line;
        
        string Curr() => src[start..end];

        Lst<Token> AddToken(TokenType type)
            => AddLiteral(type, Nothing);
        
        Lst<Token> AddLiteral(TokenType type, Maybe<Literal> literal)
            => tokens.Append(new Token(type, Curr(), literal, line));

        char Advance() => src[end++];
        
        if (src == "") return (state, tokens.Append(new Token(EOF, "", Nothing, line)));

        return Advance() switch {
            '(' => ((src + Curr(), line), AddToken(L_PAREN)),
            ')' => ((src + Curr(), line), AddToken(R_PAREN)),
            '{' => ((src + Curr(), line), AddToken(L_CURLY)),
            '}' => ((src + Curr(), line), AddToken(R_CURLY)),
            ',' => ((src + Curr(), line), AddToken(COMMA)),
            '.' => ((src + Curr(), line), AddToken(DOT)),
            '-' => ((src + Curr(), line), AddToken(MINUS)),
            '+' => ((src + Curr(), line), AddToken(PLUS)),
            ';' => ((src + Curr(), line), AddToken(SEMICOLON)),
            '*' => ((src + Curr(), line), AddToken(ASTERIK)),
        };

    }
}