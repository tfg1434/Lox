namespace Lox;

abstract record Expr;

record Binary(Expr Left, Token Operator, Expr Right) : Expr;

record Grouping(Expr Expr) : Expr;

record LiteralExpr(Literal Value) : Expr;

record Unary(Token Operator, Expr Right) : Expr;


