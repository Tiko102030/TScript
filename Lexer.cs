using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

public class Lexer
{
    private readonly string _source;

    public Lexer(string source)
    {
        _source = source;
    }

    public string SplitIntoChunks(string source)
    {
        return source;
    }

    public void Tokenize()
    {
        string source_line = SplitIntoChunks(_source);
        Console.WriteLine("Tokenizing: " + source_line + "\n");

        source_line = source_line.Trim();

        int i = 0;
        while(i < source_line.Length)
        {
            char letter = source_line[i];
            string token;
            int start; // start position of a token

            if(char.IsLetter(letter))
            {
                start = i;

                while(i < source_line.Length && char.IsLetter(source_line[i]))
                    i++;
                
                token = source_line.Substring(start, i - start);
                Console.WriteLine("Token is a string: (" + token + ")");
            }
            else if(char.IsSymbol(letter))
            {
                start = i;

                while(i < source_line.Length && char.IsSymbol(source_line[i]))
                    i++;

                token = source_line.Substring(start, i - start);
                Console.WriteLine("Token is a symbol: (" + token + ")");
            }
            else if(char.IsDigit(letter))
            {
                start = i;

                while(i < source_line.Length && (char.IsDigit(source_line[i]) || source_line[i] == '.'))
                    i++;

                token = source_line.Substring(start, i - start);
                Console.WriteLine("Token is a number: (" + token + ")");
            }
            else if(letter == ';')
            {
                token = letter.ToString();
                Console.WriteLine("Token is a ';': (" + token + ")");
                i++;
            }
            else if(char.IsWhiteSpace(letter))
            {
                token = letter.ToString();
                Console.WriteLine("Token is white space: (" + token + ")");
                i++;
            }
            else
            {
                Console.WriteLine("UNSUPPORTED CHAR: " + letter);  
                i++;              
            }
        }

        Console.Write("\n");
    }
}