using System.Runtime.InteropServices;

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

    string GetDeclarationKeywords()
    {
        string s = "";

        foreach(VarType type in Enum.GetValues(typeof(VarType)))
        {
            s += type.ToString();
        }

        return s.ToLower();
    }

    Statement ParseStatement()
    {
        Statement statement = null;

        // Checks if the token is a variable type
        if(GetDeclarationKeywords().Contains(tokenList[i].Lexeme))
        {
            VarType type = GetVarType(tokenList[i++].Lexeme);

            string name = Consume(TokenType.Identifier).Lexeme;
            
            Consume(TokenType.Equals);

            Expression declaration = ParseExpression();
            statement = new VarDeclaration(type, name, declaration);
        }
        else
        {
            throw new Exception("Couldn't parse statement");
        }

        if(tokenList[i].Type == TokenType.Semicolon)
        {
            i++;
            return statement;
        }
        else
        {
            throw new Exception("Couldn't parse statement");
        }
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
}