class Program
{
    static void Main()
    {
        var lexer = new Lexer("цел x = (1.55 * 2);");
        lexer.Tokenize();
    }
}