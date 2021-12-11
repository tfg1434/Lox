namespace Lox; 

static class AstPrinter {
    public static string Print(Expr expr) => expr switch {
        Binary binary => $"({Print(binary.Left)} {binary.Operator.Lexeme} {Print(binary.Right)})",
        Grouping grouping => $"({Print(grouping.Expr)})",
        LiteralExpr literal => literal.Value.ToString(),
        Unary unary => $"({unary.Operator.Lexeme} {Print(unary.Right)})",
        _ => throw new InvalidOperationException("wtf"),
    };
}