abstract class ASTnode() {}
abstract class Statement : ASTnode {}
abstract class Expression : ASTnode {}

class Parser
{   
    public int i = 0; // current token ID

    private readonly List<Token> tokenList;

    public Parser(List<Token> _tokenList)
    {
        tokenList = _tokenList;
    }

    public void Parse()
    {
        foreach (var token in tokenList)
        {
            Console.WriteLine("[{0}, {1}]", token.Type, token.Lexeme); 
        }

        while(i < tokenList.Count())
        {
            


        }




    }

    Token Next()
    {
        i++;
        return tokenList[i];
    }

    Token Peek()
    {
        return tokenList[i+1];
    }

    (float l, float r) GetBindingPower(string op)
    {
        if(op == "+" || op == "-")
        {
            return (l: 1, r: 1.1f);
        }
        else if(op == "*" || op == "/")
        {
            return (l: 2, r: 2.1f);
        }
        else
        {
            Console.WriteLine("Error assigning binding power");
            return (l: 0, r: 0);
        }

    }
}