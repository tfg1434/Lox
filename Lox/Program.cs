using FunctionalSharp.Wrappers;
using Lox;
using static Lox.Lox;
using static FunctionalSharp.Wrappers.File<FunctionalSharp.Wrappers.LiveRuntime>;
using static FunctionalSharp.Wrappers.Console<FunctionalSharp.Wrappers.LiveRuntime>;

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

static IO<LiveRuntime, Maybe<Lst<CodeError>>> RunFile(string path)
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

