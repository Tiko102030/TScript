using System.Text;

class ASTprinter
{
    private StringBuilder sb = new StringBuilder();

    public string Print(List<Statement> program)
    {
        sb.Clear();
        PrintProgram(program, 0);
        return sb.ToString();
    }

    private void PrintProgram(List<Statement> program, int indent)
    {
        WriteLine(indent, "Program");

        foreach (var stmt in program)
        {
            PrintStatement(stmt, indent + 1);
        }
    }

    private void PrintStatement(Statement stmt, int indent)
    {
        switch (stmt)
        {
            case VarDeclaration varDecl:
                PrintVarDeclaration(varDecl, indent);
                break;

            default:
                WriteLine(indent, $"Unknown Statement ({stmt.GetType().Name})");
                break;
        }
    }

    private void PrintVarDeclaration(VarDeclaration decl, int indent)
    {
        WriteLine(indent, $"VarDeclaration");
        WriteLine(indent + 1, $"Type: {decl.Type}");
        WriteLine(indent + 1, $"Name: {decl.Name}");
        WriteLine(indent + 1, "Initializer:");
        PrintExpression(decl.Declaration, indent + 2);
    }

    private void PrintExpression(Expression expr, int indent)
    {
        switch (expr)
        {
            case BinaryExpression bin:
                WriteLine(indent, $"BinaryExpression ({bin.Op.Lexeme})");
                PrintExpression(bin.Left, indent + 1);
                PrintExpression(bin.Right, indent + 1);
                break;

            case NumberExpression num:
                WriteLine(indent, $"Number ({num.Value})");
                break;

            default:
                WriteLine(indent, $"Unknown Expression ({expr.GetType().Name})");
                break;
        }
    }

    private void WriteLine(int indent, string text)
    {
        sb.AppendLine(new string(' ', indent * 2) + text);
    }
}
