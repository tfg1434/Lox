using FunctionalSharp.Wrappers;
using Lox;
using static Lox.Lox;
using static Lox.TokenType;
using static FunctionalSharp.Wrappers.File<FunctionalSharp.Wrappers.LiveRuntime>;
using static FunctionalSharp.Wrappers.Console<FunctionalSharp.Wrappers.LiveRuntime>;

Expr expr = new Binary(
    new Unary(
        new Token(MINUS, "-", Nothing, 1),
        new LiteralExpr(123)),
    new Token(ASTERIK, "*", Nothing, 1),
    new Grouping(
        new LiteralExpr(45.67)));

Console.WriteLine(AstPrinter.Print(expr));


switch (args.Length) {
    case > 1:
        WriteLine("Usage: lox [file]");
        Environment.Exit(64);
        
        break;
    case 1:
        //e is IOError
        RunFile(args[0]).Run(new()).Match(
            ioError => WriteLine(ioError.Message),
            mCodeErrors => mCodeErrors.Match(
                () => WriteLine("No errors"),
                codeErrors => WriteLine("Compilation failed."))
            );

        break;
    default:
        //RunPrompt();

        break;

}

static IO<LiveRuntime, Maybe<Lst<LexError>>> RunFile(string path)
    => from src in ReadAllText(path)
       select Run(src);

// static Result<Lst<IOError>> RunPrompt() {
//     while (true) {
//         from _ in WriteLine(">> ")
//         from line in ReadLine()
//         from d in line is null
//             ? EffFail<Unit>(new QuitReplError())
//             : EffSucc(Unit())
//         select Run(line);
//     }
// }

