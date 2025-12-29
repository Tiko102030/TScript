class ASTprinter
{
    public string Print(Expression expr)
    {
        return expr switch
        {
            NumberExpression n => n.Value.ToString(),
            BinaryExpression b => Parenthesize(b.Op.Lexeme, b.Left, b.Right),
            _ => "<?>"
        };
    }

    string Parenthesize(string name, Expression left, Expression right)
    {
        return $"({name} {Print(left)} {Print(right)})"; 
    }
}
