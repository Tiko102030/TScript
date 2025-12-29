abstract class ASTnode {}
abstract class Statement : ASTnode {}
abstract class Expression : ASTnode {}

class BinaryExpression : Expression
{
    public Expression Left, Right;
    public Token Op;

    public BinaryExpression(Expression left, Token op, Expression right)
    {
        Left = left;
        Op = op;
        Right = right;
    }
}

class NumberExpression : Expression
{
    public double Value;

    public NumberExpression(double value)
    {
        Value = value;
    }
}

class VarDeclaration : Statement
{
    public VarType Type;
    public string Name;
    public Expression Declaration;

    public VarDeclaration(VarType type, string name, Expression declaration)
    {
        Type = type;
        Name = name;
        Declaration = declaration;
    }
}

class Parser
{   
    public int i = 0; // current token ID

    private readonly List<Token> tokenList;

    public Parser(List<Token> _tokenList)
    {
        tokenList = _tokenList;
    }

    Token Consume(TokenType type)
    {
        if(tokenList[i].Type == type)
        {   
            return tokenList[i++];
        }

        throw new Exception($"Expected {type}, got {tokenList[i].Type}");
    }

    public void ParseProgram()
    {
        List<Statement> statements = new List<Statement>();

        while(tokenList[i].Type != TokenType.EOF)
        {
            statements.Add(ParseStatement());
        }
    }

    Statement ParseStatement()
    {
        if(tokenList[i].Type == TokenType.Keyword)
        {
            VarType type = GetVarType(tokenList[i].Lexeme);
            
            i++;
            string name;
            if(tokenList[i].Type == TokenType.Identifier)
            {
                name = tokenList[i].Lexeme;
            }
            else
            {
                throw new Exception("Expected identifier");
            }
            
            i++;
            if(tokenList[i].Type != TokenType.Equals)
            {
                throw new Exception("Expected '=' when declaring variable");
            }

            i++;
            Expression declaration = ParseExpression();

            return new VarDeclaration(type, name, declaration);
        }


        throw new Exception("Couldn't parse Expression");
    }

    public Expression ParseExpression()
    {
        Expression expr = ParseTerm();

        while(tokenList[i].Type == TokenType.Plus || tokenList[i].Type == TokenType.Minus) // + or -
        {
            Token op = tokenList[i++];
            Expression right = ParseTerm();
            expr = new BinaryExpression(expr, op, right);
        }

        return expr;
    }

    Expression ParseTerm()
    {
        Expression expr = ParseFactor();

        while(tokenList[i].Type == TokenType.Star || tokenList[i].Type == TokenType.Slash) // * or /
        {
            Token op = tokenList[i++];
            Expression right = ParseFactor();
            expr = new BinaryExpression(expr, op, right);
        }

        return expr;
    }

    Expression ParseFactor()
    {
        if(tokenList[i].Type == TokenType.Number)
        {
            double value = Convert.ToDouble(tokenList[i].Lexeme);
            i++;
            return new NumberExpression(value);
        }

        if(tokenList[i].Type == TokenType.LeftParen)
        {
            i++;
            Expression expr = ParseExpression();
            Consume(TokenType.RightParen);
            return expr;
        }

        throw new Exception("Expected expression");
    }

    VarType GetVarType(string s)
    {
        foreach(VarType type in Enum.GetValues(typeof(VarType)))
        {
            if(type.ToString().ToLower() == s)
            {
                return type;
            }
        }

        throw new Exception($"{s} is not a valid Variable Type");
    }
}