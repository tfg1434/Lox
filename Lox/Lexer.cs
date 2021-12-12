using System.Diagnostics.CodeAnalysis;
using Lox;
using static Lox.TokenType;

namespace Lox; 

class Lexer {
    private static readonly Map<string, TokenType> _keywords = Map(
        ("and", AND),
        ("class", CLASS),
        ("else", ELSE),
        ("false", FALSE),
        ("for", FOR),
        ("fun", FUN),
        ("if", IF),
        ("nil", NIL),
        ("or", OR),
        ("print", PRINT),
        ("return", RETURN),
        ("base", BASE),
        ("this", THIS),
        ("true", TRUE),
        ("var", VAR),
        ("while", WHILE)
    );
    
    private readonly string _src;
    
    public Lexer(string src) => _src = src;

    public (Lst<Token> Tokens, Lst<LexError> Errors) Lex() {
        (_, Lst<Token> tokens, Lst<LexError> errors) = LexRec((_src, 0), Lst<Token>.Empty, Lst<LexError>.Empty);

        return (Tokens: tokens, Errors: errors);
    }
    
    private ((string Str, int Line) State, Lst<Token> Tokens, Lst<LexError> Errors) 
        LexRec((string Str, int Line) state, Lst<Token> tokens, Lst<LexError> errors) {

        int start = 0;
        int end = 0;
        string src = state.Str;
        int line = state.Line;

        if (src == "") return (state, tokens.Append(new Token(EOF, "", Nothing, line)), errors);
        
        char c = Advance();

        switch (c) { 
            case '(': return AddToken(L_PAREN);
            case ')': return AddToken(R_PAREN);
            case '{': return AddToken(L_CURLY);
            case '}': return AddToken(R_CURLY);
            case ',': return AddToken(COMMA);
            case '.': return AddToken(DOT);
            case '-': return AddToken(MINUS);
            case '+': return AddToken(PLUS);
            case ';': return AddToken(SEMICOLON);
            case '*': return AddToken(ASTERIK);
            case '!': return AddToken(Match('=') ? BANG_EQUAL : BANG);
            case '=': return AddToken(Match('=') ? EQUAL_EQUAL : EQUAL);
            case '<': return AddToken(Match('=') ? LESS_EQUAL : LESS);
            case '>': return AddToken(Match('=') ? GREATER_EQUAL : GREATER);
            case '/':
                if (Match('/')) {
                    AdvanceWhile(x => x != '\n' && !IsEnd());

                    return DefaultBut();
                }
                else
                    return AddToken(SLASH);
            case ' ' or '\r' or '\t': return DefaultBut();
            case '\n': return DefaultBut(Line: line + 1);
            case '"':
                AdvanceWhile(x => x != '"');

                if (IsEnd())
                    return Report(new(line, "Unterminated string."));

                //closing "
                Advance();
                
                string value = src[(start + 1)..(end - 1)];
                //                            generic fun
                return AddLiteral(STRING, Just((Literal) value));
            case >= '0' and <= '9':
                AdvanceWhile(char.IsDigit);

                Maybe<char> peekNext;
                if (Peek() == '.' && (peekNext = PeekNext()).IsJust && char.IsDigit(peekNext.NotNothing())) {
                    //consume .
                    Advance();
                    
                    AdvanceWhile(char.IsDigit);
                }

                return AddLiteral(NUMBER, Just((Literal) double.Parse(Curr())));
            case >= 'a' and <= 'z' or >= 'A' and <= 'Z' or '_':
                AdvanceWhile(x => char.IsDigit(x) || x is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or '_');

                string txt = src[start..end];
                Maybe<TokenType> type = _keywords.Get(txt);

                return AddToken(type.Match(() => IDENTIFIER, t => t));
            default: 
                return Report(new(line, $"Unexpected character {c}."));
        }
        
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        ((string Str, int Line) State, Lst<Token> Tokens, Lst<LexError> Errors) 
            DefaultBut(string? Src = null, int? Line = null, Lst<Token>? Tokens = null, Lst<LexError>? Errors = null)
        
            => LexRec((Src ?? src[end..], Line ?? line), Tokens ?? tokens, Errors ?? errors);
        
        string Curr() => src[start..end];

        ((string Str, int Line) State, Lst<Token> Tokens, Lst<LexError> Errors) AddToken(TokenType type)
            => AddLiteral(type, Nothing);
        
        ((string Str, int Line) State, Lst<Token> Tokens, Lst<LexError> Errors)
            AddLiteral(TokenType type, Maybe<Literal> literal)
        
            => DefaultBut(Tokens: tokens.Append(new Token(type, Curr(), literal, line)));

        ((string Str, int Line) State, Lst<Token> Tokens, Lst<LexError> Errors)
            Report(LexError error)

            => DefaultBut(Line: error.Line, Errors: errors.Append(error));

        char Advance() => src[end++];

        string AdvanceWhile(Func<char, bool> p) {
            string s = "";

            Maybe<char> peek;
            while ((peek = Peek()).IsJust && p(peek.NotNothing())) {
                if (peek.NotNothing() == '\n') line++;
                s += Advance();
            }
            
            return s;
        }
        
        bool Match(char match) {
            if (IsEnd()) return false;
            if (src[end] != match) return false;

            end++;

            return true;
        }

        Maybe<char> Peek() => IsEnd() ? Nothing : Just(src[end]);
        
        Maybe<char> PeekNext() => IsEnd() ? Nothing : Just(src[end + 1]);
        
        bool IsEnd() => end >= src.Length;
    }
}