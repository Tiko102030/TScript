class Program
{
    static void Main()
    {
        var lexer = new Lexer("цел x = (1.55 * 2);");
        var parser = new Parser(lexer.Tokenize());
        parser.ParseExpression();
    }
}