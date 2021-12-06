using static Lox.TokenType;

namespace Lox; 

class Lexer {
    private readonly string _src;
    
    public Lexer(string src)
        => _src = src;

    private ((string Str, int Line) State, Lst<Token> Tokens, Lst<CodeError> Errors) 
        LexRec((string Str, int Line) state, Lst<Token> tokens, Lst<CodeError> errors) {

        int start = 0;
        int end = 0;
        string src = state.Str;
        int line = state.Line;
        
        if (src == "") return (state, tokens.Append(new Token(EOF, "", Nothing, line)), errors);
        
        string Curr() => src[start..end];

        ((string Str, int Line) State, Lst<Token> Tokens, Lst<CodeError> Errors) AddToken(TokenType type)
            => AddLiteral(type, Nothing);
        
        ((string Str, int Line) State, Lst<Token> Tokens, Lst<CodeError> Errors)
            AddLiteral(TokenType type, Maybe<Literal> literal)
        
        //TODO: Subtract, not add Curr()
            => ((src + Curr(), line), tokens.Append(new Token(type, Curr(), literal, line)), errors);

        ((string Str, int Line) State, Lst<Token> Tokens, Lst<CodeError> Errors)
            Report(CodeError error)

            => ((src + Curr(), error.Line), tokens, errors.Append(error));

        char Advance() => src[end++];

        Unit AdvanceWhile(Func<char, bool> p) {
            while (p(Peek()) && !IsEnd()) Advance();

            return Unit();
        }
        
        bool Match(char c) {
            if (IsEnd()) return false;
            if (src[end] != c) return false;

            end++;

            return true;
        }
        
        bool IsEnd() => end >= src.Length;

        char c = Advance();
        return c switch {
            '(' => AddToken(L_PAREN),
            ')' => AddToken(R_PAREN),
            '{' => AddToken(L_CURLY),
            '}' => AddToken(R_CURLY),
            ',' => AddToken(COMMA),
            '.' => AddToken(DOT),
            '-' => AddToken(MINUS),
            '+' => AddToken(PLUS),
            ';' => AddToken(SEMICOLON),
            '*' => AddToken(ASTERIK),
            '!' => AddToken(Match('=') ? BANG_EQUAL : BANG),
            '=' => AddToken(Match('=') ? EQUAL_EQUAL : EQUAL),
            '<' => AddToken(Match('=') ? LESS_EQUAL : LESS),
            '>' => AddToken(Match('=') ? GREATER_EQUAL : GREATER),
            '/'
            
            _ => Report(new(line, $"Unexpected character {c}.")),
        };

    }
}