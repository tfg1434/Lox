namespace Lox;

enum TokenType {
    //single chars
    L_PAREN, R_PAREN, L_CURLY, R_CURLY, COMMA, DOT,
    MINUS, PLUS, SEMICOLON, SLASH, ASTERIK,

    //one or two char
    BANG, BANG_EQUAL, EQUAL, EQUAL_EQUAL, GREATER,
    GREATER_EQUAL, LESS, LESS_EQUAL,

    //literals
    IDENTIFIER, STRING, NUMBER,

    //keywords
    AND, OR, CLASS, IF, ELSE, TRUE, FALSE, FUN, FOR, NIL,
    PRINT, RETURN, BASE, THIS, VAR, WHILE,

    EOF,
}

record class Token(TokenType Type, string Lexeme, Maybe<Literal> Literal, int Line);