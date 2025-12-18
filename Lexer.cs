using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

public class Lexer
{
    private readonly string _source;
    public List<Token> tokenList = new List<Token>();

    public Lexer(string source)
    {
        _source = source;
    }

    public string SplitIntoChunks(string source)
    {
        return source;
    }

    public List<Token> Tokenize()
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

                string[] possibleKeywords = ["цел", "функ"];

                if (Array.Exists(possibleKeywords, t => t == token))
                {
                    tokenList.Add(new Token(TokenType.Keyword, token));
                }
                else
                {
                    tokenList.Add(new Token(TokenType.Identifier, token));
                }
            }
            else if(char.IsSymbol(letter) || letter == '/' || letter == '*')
            {
                start = i;

                while(i < source_line.Length && (char.IsSymbol(source_line[i]) || source_line[i] == '/' || source_line[i] == '*'))
                    i++;

                token = source_line.Substring(start, i - start);

                switch(token)
                {
                    case "+":
                    tokenList.Add(new Token(TokenType.Plus, token));
                    break;

                    case "-":
                    tokenList.Add(new Token(TokenType.Minus, token));
                    break;

                    case "=":
                    tokenList.Add(new Token(TokenType.Equals, token));
                    break;

                    case "/":
                    tokenList.Add(new Token(TokenType.Slash, token));
                    break;

                    case "*":
                    tokenList.Add(new Token(TokenType.Star, token));
                    break;

                    default:
                    Console.WriteLine("\nUNSUPPORTED CHAR: " + token + "\n"); 
                    break; 
                }
            }
            else if(char.IsDigit(letter))
            {
                start = i;

                while(i < source_line.Length && (char.IsDigit(source_line[i]) || source_line[i] == '.'))
                    i++;

                token = source_line.Substring(start, i - start);
                tokenList.Add(new Token(TokenType.Number, token));
            }
            else if(letter == ';')
            {
                token = letter.ToString();
                i++;
                tokenList.Add(new Token(TokenType.Semicolon, token));
            }
            else if(char.IsWhiteSpace(letter))
            {
                token = letter.ToString();
                // Console.WriteLine("Token is white space: (" + token + ")");
                i++;

            }
            else if(letter == '(')
            {
                token = letter.ToString();
                i++;
                tokenList.Add(new Token(TokenType.LeftParen, token));
            }
            else if(letter == ')')
            {
                token = letter.ToString();
                i++;
                tokenList.Add(new Token(TokenType.RightParen, token));
            }
            else
            {
                Console.WriteLine("\nUNSUPPORTED CHAR: " + letter + "\n");  
                i++;              
            }
        }

        tokenList.Add(new Token(TokenType.EOF, ""));
        
        foreach (var token in tokenList)
        {
            Console.WriteLine("[{0}, {1}]", token.Type, token.Lexeme); 
        }

        Console.Write("\n");

        return tokenList;
    }
}