using System.Reflection;
using System.Text;

class ASTprinter
{
    public string Print(List<Statement> statements)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < statements.Count; i++)
        {
            bool isLast = i == statements.Count - 1;
            PrintNode(statements[i], sb, "", isLast);
        }

        return sb.ToString();
    }

    private void PrintNode(object node, StringBuilder sb, string prefix, bool isLast)
    {
        if (node == null)
            return;

        sb.Append(prefix);
        sb.Append(isLast ? "└── " : "├── ");
        sb.AppendLine(node.GetType().Name);

        var fields = node.GetType()
                         .GetFields(BindingFlags.Public | BindingFlags.Instance);

        string childPrefix = prefix + (isLast ? "    " : "│   ");

        for (int i = 0; i < fields.Length; i++)
        {
            var field = fields[i];
            object value = field.GetValue(node);

            if (value == null)
                continue;

            bool lastField = i == fields.Length - 1;

            // AST node → recurse
            if (value is ASTnode)
            {
                sb.Append(childPrefix);
                sb.AppendLine(field.Name + ":");
                PrintNode(value, sb, childPrefix, lastField);
            }
            // List of AST nodes (future-proof)
            else if (value is IEnumerable<ASTnode> list)
            {
                sb.Append(childPrefix);
                sb.AppendLine(field.Name + ":");

                var items = list.ToList();
                for (int j = 0; j < items.Count; j++)
                {
                    PrintNode(items[j], sb, childPrefix, j == items.Count - 1);
                }
            }
            // Primitive / enum / token
            else
            {
                sb.Append(childPrefix);
                sb.Append(isLast ? "└── " : "├── ");
                sb.AppendLine($"{field.Name}: {value}");
            }
        }
    }
}
