abstract class ASTnode { }

abstract class Statement : ASTnode { }

abstract class Expression : ASTnode { }

class FunctionParameter
{
    public VarType Type;
    public string Name;

    public FunctionParameter(VarType type, string name)
    {
        Type = type;
        Name = name;
    }
}

class BinaryExpression : Expression
{
    public Expression Left,
        Right;
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

class StringExpression : Expression
{
    public string Value;

    public StringExpression(string value)
    {
        Value = value;
    }
}

class VariableExpression : Expression
{
    public Token Name;

    public VariableExpression(Token name)
    {
        Name = name;
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

class VarAssignment : Statement
{
    public string Name;
    public Expression Assignment;

    public VarAssignment(string name, Expression assignment)
    {
        Name = name;
        Assignment = assignment;
    }
}

class FunctionDeclaration : Statement
{
    public string Name;
    public List<FunctionParameter> Parameters;
    public List<Statement> FunctionContents;

    public FunctionDeclaration(
        string name,
        List<FunctionParameter> parameters,
        List<Statement> functionContents
    )
    {
        Name = name;
        Parameters = parameters;
        FunctionContents = functionContents;
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
        if (tokenList[i].Type == type)
        {
            return tokenList[i++];
        }

        throw new Exception($"Expected {type}, got {tokenList[i].Type}");
    }

    public List<Statement> ParseProgram()
    {
        List<Statement> statements = new List<Statement>();

        while (tokenList[i].Type != TokenType.EOF)
        {
            statements.Add(ParseStatement());
        }

        return statements;
    }

    VarType GetVarType(string s)
    {
        foreach (VarType type in Enum.GetValues(typeof(VarType)))
        {
            if (type.ToString().ToLower() == s)
            {
                return type;
            }
        }

        throw new Exception($"{s} is not a valid Variable Type");
    }

    bool Match(TokenType type)
    {
        if (tokenList[i].Type == type)
        {
            i++;
            return true;
        }
        return false;
    }

    Statement ParseStatement()
    {
        // Statement statement = null;
        Statement statement;

        // Checks if the token is a variable type
        if (Enum.TryParse<VarType>(tokenList[i].Lexeme, ignoreCase: true, out var c)) // Var Declaration
        {
            VarType type = GetVarType(tokenList[i++].Lexeme);

            string name = Consume(TokenType.Identifier).Lexeme;

            Consume(TokenType.Equals);

            Expression declaration = ParseExpression();
            statement = new VarDeclaration(type, name, declaration);
        }
        else if (tokenList[i].Type == TokenType.Identifier) // Var Assignment
        {
            string name = Consume(TokenType.Identifier).Lexeme;

            Consume(TokenType.Equals);

            Expression assignment = ParseExpression();
            statement = new VarAssignment(name, assignment);
        }
        else if (tokenList[i].Lexeme == "функ")
        {
            i++;
            string name = Consume(TokenType.Identifier).Lexeme;

            Consume(TokenType.LeftParen);

            List<FunctionParameter> parameters = new();

            if (tokenList[i].Type != TokenType.RightParen)
            {
                do
                {
                    Token typeToken = Consume(TokenType.Keyword);
                    VarType type = GetVarType(typeToken.Lexeme);

                    string par_name = Consume(TokenType.Identifier).Lexeme;
                    parameters.Add(new FunctionParameter(type, par_name));
                } while (Match(TokenType.Comma));
            }

            Consume(TokenType.RightParen);

            Consume(TokenType.LeftCurlyParen);

            List<Statement> function_statements = new List<Statement>();

            while (tokenList[i].Type != TokenType.RightCurlyParen)
            {
                if (tokenList[i].Type == TokenType.EOF)
                {
                    throw new Exception("Missing '}'");
                }
                function_statements.Add(ParseStatement());
            }

            Consume(TokenType.RightCurlyParen);

            statement = new FunctionDeclaration(name, parameters, function_statements);
        }
        else
        {
            throw new Exception("Couldn't parse statement");
        }

        if (statement is FunctionDeclaration)
            return statement;

        if (tokenList[i].Type == TokenType.Semicolon)
        {
            i++;
            return statement;
        }
        else
        {
            throw new Exception("Couldn't parse statement, expected semicolon");
        }
    }

    public Expression ParseExpression()
    {
        Expression expr = ParseTerm();

        while (tokenList[i].Type == TokenType.Plus || tokenList[i].Type == TokenType.Minus) // + or -
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

        while (tokenList[i].Type == TokenType.Star || tokenList[i].Type == TokenType.Slash) // * or /
        {
            Token op = tokenList[i++];
            Expression right = ParseFactor();
            expr = new BinaryExpression(expr, op, right);
        }

        return expr;
    }

    Expression ParseFactor()
    {
        if (tokenList[i].Type == TokenType.Number)
        {
            double value = Convert.ToDouble(tokenList[i++].Lexeme);
            return new NumberExpression(value);
        }

        if (tokenList[i].Type == TokenType.Identifier)
        {
            Token name = tokenList[i++];
            return new VariableExpression(name);
        }

        if (tokenList[i].Type == TokenType.LeftParen)
        {
            i++;
            Expression expr = ParseExpression();
            Consume(TokenType.RightParen);
            return expr;
        }

        if (tokenList[i].Type == TokenType.String)
        {
            string value = tokenList[i++].Lexeme;
            return new StringExpression(value);
        }

        throw new Exception("Expected expression");
    }
}

