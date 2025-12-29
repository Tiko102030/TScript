class Program
{
    static void Main()
    {
        var lexer = new Lexer("(2 * (1.55 + 3.2)) * 2");
        var parser = new Parser(lexer.Tokenize());

        Expression ast = parser.ParseExpression();

        ASTprinter printer = new ASTprinter();
        Console.WriteLine(printer.Print(ast));
    }
}