class Program
{
    static void Main()
    {
        string inputFile = File.ReadAllText("main.tscript");

        var lexer = new Lexer(inputFile);
        var parser = new Parser(lexer.Tokenize());

        List<Statement> ast = parser.ParseProgram();

        ASTprinter printer = new ASTprinter();
        Console.WriteLine(printer.Print(ast));
    }
}