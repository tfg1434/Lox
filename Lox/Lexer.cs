using System.Diagnostics.CodeAnalysis;
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
        
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        ((string Str, int Line) State, Lst<Token> Tokens, Lst<CodeError> Errors) 
            DefaultBut(string? Src = null, int? Line = null, Lst<Token>? Tokens = null, Lst<CodeError>? Errors = null)
        
            => ((Src ?? src + Curr(), Line ?? line), Tokens ?? tokens, Errors ?? errors);
        
        string Curr() => src[start..end];

        ((string Str, int Line) State, Lst<Token> Tokens, Lst<CodeError> Errors) AddToken(TokenType type)
            => AddLiteral(type, Nothing);
        
        ((string Str, int Line) State, Lst<Token> Tokens, Lst<CodeError> Errors)
            AddLiteral(TokenType type, Maybe<Literal> literal)
        
            => DefaultBut(Tokens: tokens.Append(new Token(type, Curr(), literal, line)));

        ((string Str, int Line) State, Lst<Token> Tokens, Lst<CodeError> Errors)
            Report(CodeError error)

            => DefaultBut(Line: error.Line, Errors: errors.Append(error));

        char Advance() => src[end++];

        string AdvanceWhile(Func<char, bool> p) {
            string s = "";

            Maybe<char> peek = Peek();
            while (peek.IsJust && p(peek.NotNothing())) 
                s += Advance();

            return s;
        }
        
        bool Match(char c) {
            if (IsEnd()) return false;
            if (src[end] != c) return false;

            end++;

            return true;
        }

        Maybe<char> Peek() => IsEnd() ? Nothing : Just(src[end]);
        
        bool IsEnd() => end >= src.Length;

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
                if (Match('/'))
                    AdvanceWhile(x => x != '\n' && !IsEnd());
                else
                    return AddToken(SLASH);

                break;
            case ' ' or '\r' or '\t': break;
            case '\n': return DefaultBut(Line: line + 1);
            
        }
    }
}