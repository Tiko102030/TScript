using System.Linq.Expressions;

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
            TypeCode type = letter.GetTypeCode();

            string token;

            if(type == TypeCode.String)
            {
                token = letter.ToString();
                while(type == TypeCode.String && i < 100)
                {
                    letter = source_line[++i];
                    type = letter.GetTypeCode();
                    token += letter.ToString();
                    i++;
                }
                Console.WriteLine("The token is: " + token);
            }
            else if (type == TypeCode.Int16 || type == TypeCode.Int32)
            {
                
            }

            Console.WriteLine("type is: " + type);
            i++;
        }
        
        
        
        
        
        
        
        
        
        // char[] split_points = [' '];

        // string[] chunks = source_line.Split(split_points);

        // foreach(string chunk in chunks)
        //     Console.WriteLine(chunk);



        // for(int i = 0; i < source_line.Length; i++)
        // {
        //     Console.WriteLine(source_line[i]);
        // }
    }
}