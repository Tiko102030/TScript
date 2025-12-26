using System.Runtime.InteropServices;

abstract class ASTnode() {}
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

class Parser
{   
    public int i = 0; // current token ID

    private readonly List<Token> tokenList;

    public Parser(List<Token> _tokenList)
    {
        tokenList = _tokenList;
        foreach (var token in tokenList)
        {
            Console.WriteLine("[{0}, {1}]", token.Type, token.Lexeme); 
        }
    }

    Token Consume(TokenType type)
    {
        if(tokenList[i].Type == type)
        {
            i++;
            return tokenList[i];
        }

        throw new Exception($"Expected {type}, got {Peek().Type}");
    }

    public Expression ParseExpression()
    {
        Expression expr = ParseTerm();

        while(tokenList[i].Type == TokenType.Plus || tokenList[i].Type == TokenType.Minus) // + or -
        {
            Token op = tokenList[--i];
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
            Token op = tokenList[--i];
            Expression right = ParseExpression();
            expr = new BinaryExpression(expr, op, right);
        }

        return expr;
    }

    Expression ParseFactor()
    {
        if(tokenList[i].Type == TokenType.Number)
        {
            return new NumberExpression(Convert.ToDouble(tokenList[--i].Lexeme));
        }

        if(tokenList[i].Type == TokenType.LeftParen)
        {
            Expression expr = ParseExpression();
            Consume(TokenType.RightParen);
            return expr;
        }

        throw new Exception("Factor couldn't be parsed");

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
}