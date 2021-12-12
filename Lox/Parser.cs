using static Lox.TokenType;

namespace Lox;

class Parser {
    private readonly Lst<Token> _tokens;

    public Parser(Lst<Token> tokens) => _tokens = tokens;

    private (Lst<Token> State, Expr Tree) ParseRec(Lst<Token> state, Expr tree) {
        int curr = 0;
        
        
        
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

        Expr ParseFactor() {
            Expr expr = ParseUnary();
            
            while (Match(SLASH, ASTERIK)) {
                Token op = Previous();
                Expr right = ParseUnary();
                expr = new Binary(expr, op, right);
            }
            
            return expr;
        }

        Expr ParseUnary() {
            if (!Match(BANG, MINUS)) return ParsePrimary();
            
            Token op = Previous();
            Expr right = ParseUnary();
            return new Unary(op, right);
        }

        Expr ParsePrimary() {
            if (Match(FALSE)) return new LiteralExpr(false);
            if (Match(TRUE)) return new LiteralExpr(true);
            if (Match(NIL)) return new LiteralExpr(null);
            
            
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

        bool IsEnd() => Peek().Type == EOF;
        
        Token Peek() => _tokens[curr];
        
        Token Previous() => _tokens[curr - 1];
    }
}