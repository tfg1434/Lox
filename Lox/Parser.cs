using static Lox.Lox;
using static Lox.TokenType;

namespace Lox;

class Parser {
    private readonly Lst<Token> _tokens;

    public Parser(Lst<Token> tokens) => _tokens = tokens;

    private (Lst<Token> State, Expr Tree) ParseRec(Lst<Token> state, Expr tree) {
        int curr = 0;


        Unit Synchronize() {
            Advance();

            AdvanceWhile(token => Previous().Type != SEMICOLON
                                  && token.Type is not CLASS or FOR or FUN or IF or PRINT or RETURN or VAR or WHILE);

            Advance();

            return Unit();
        }
        
        Expr ParseExpr() => ParseEquality();

        Expr ParseEquality() {
            Expr expr = ParseComparison();
            
            while (Match(BANG_EQUAL, EQUAL_EQUAL)) {
                Token op = Previous();
                Expr right = ParseComparison();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        Expr ParseComparison() {
            Expr expr = ParseTerm();

            while (Match(GREATER, GREATER_EQUAL, LESS, LESS_EQUAL)) {
                Token op = Previous();
                Expr right = ParseTerm();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        Expr ParseTerm() {
            Expr expr = ParseFactor();
            
            while (Match(MINUS, PLUS)) {
                Token op = Previous();
                Expr right = ParseUnary();
                expr = new Binary(expr, op, right);
            }

            return expr;
        }

        Either<ParseError, Expr> ParseFactor() {
            Either<ParseError, Expr> expr = ParseUnary();
            
            while (Match(SLASH, ASTERIK)) {
                Token op = Previous();
                Either<ParseError, Expr> right = ParseUnary();

                expr = expr.Map(e => new Binary(e, op, right));
            }
            
            return expr;
        }

        Either<ParseError, Expr> ParseUnary() {
            if (!Match(BANG, MINUS)) return ParsePrimary();
            
            Token op = Previous();
            Either<ParseError, Expr> right = ParseUnary();
            return right.Map(expr => (Expr) new Unary(op, expr));
        }

        Either<ParseError, Expr> ParsePrimary() {
            if (Match(FALSE)) return new LiteralExpr(false);
            if (Match(TRUE)) return new LiteralExpr(true);
            if (Match(NIL)) return new LiteralExpr(null);
            if (Match(L_PAREN)) {
                Expr expr = ParseExpr();
                return Consume(R_PAREN, "Expect ')' after grouping expression.").Map(_ => (Expr) new Grouping(expr));
            }
            
            return Error(Peek(), "Expected expression.");
        }
        
        bool Match(params TokenType[] types) {
            if (!types.Any(Check)) return false;
            
            Advance();
                
            return true;
        }

        bool Check(TokenType type) {
            if (IsEnd()) return false;
            
            return Peek().Type == type;
        }

        Token Advance() {
            if (!IsEnd()) curr++;

            return Previous();
        }

        Unit AdvanceWhile(Func<Token, bool> p) {
            while (!IsEnd() && p(Peek())) 
                Advance();

            return Unit();
        }

        Either<ParseError, Token> Consume(TokenType type, string message) {
            if (Check(type)) return Advance();
            
            return Error(Peek(), message);
        }

        ParseError Error(Token token, string message) {
            if (token.Type == EOF)
                return new(token, token.Line, Report(token.Line, " at end", message));
            
            return new(token, token.Line, Report(token.Line, " at '" + token.Lexeme + "'", message));
        }

        bool IsEnd() => Peek().Type == EOF;
        
        Token Peek() => _tokens[curr];
        
        Token Previous() => _tokens[curr - 1];
    }
}