public class Lexer
{
    private readonly string _source;
    public List<Token> tokenList = new List<Token>();

    public Lexer(string source)
    {
        _source = source;
    }

    public List<Token> Tokenize()
    {
        string source = _source;
        Console.WriteLine("Tokenizing: " + source + "\n");

        source = source.Trim();

        int i = 0;
        while(i < source.Length)
        {
            char letter = source[i];
            string token;
            int start; // start position of a token

            if(letter == '#')
            {
                while(letter != '\n' && i < source.Length - 1)
                {
                    i++;
                    letter = source[i];
                }

                i++;
                if(i < source.Length)
                    letter = source[i];
                else
                    break;
            }
            
            if(char.IsLetter(letter))
            {
                start = i;

                while(i < source.Length && char.IsLetterOrDigit(source[i]) || source[i] == '_')
                    i++;
                
                token = source.Substring(start, i - start);

                string[] possibleKeywords = ["цел", "вещественное", "строка", "логическое", "функ"];

                if (Array.Exists(possibleKeywords, t => t == token))
                {
                    tokenList.Add(new Token(TokenType.Keyword, token));
                }
                else
                {
                    tokenList.Add(new Token(TokenType.Identifier, token));
                }
            }
            else if(char.IsSymbol(letter) || letter == '/' || letter == '*' || letter == '%')
            {
                token = letter.ToString();

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

                    case "%":
                    tokenList.Add(new Token(TokenType.Percent, token));
                    break;

                    default:
                    Console.WriteLine("\nUNSUPPORTED CHAR: (" + token + ")"); 
                    break; 
                }
                
                i++;
            }
            else if(char.IsDigit(letter))
            {
                start = i;

                while(i < source.Length && (char.IsDigit(source[i]) || source[i] == '.'))
                    i++;

                token = source.Substring(start, i - start);
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
                Console.WriteLine("\nUNSUPPORTED CHAR: (" + letter + ")");  
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