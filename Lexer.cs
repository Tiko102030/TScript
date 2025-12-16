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
        Console.WriteLine("Tokenizing: " + source_line);

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
                Console.WriteLine("The token is a string: (" + token + ")");
                continue;
            }
            else if(char.IsDigit(letter))
            {
                start = i;

                while(i < source_line.Length && (char.IsDigit(source_line[i]) || source_line[i] == '.'))
                    i++;

                token = source_line.Substring(start, i - start);
                Console.WriteLine("The token is a number: (" + token + ")");
                continue;
            }

            i++;
        }
    }
}