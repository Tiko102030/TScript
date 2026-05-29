using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;

class ASTprinter
{
    public string Print(List<Statement> statements)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < statements.Count; i++)
        {
            PrintNode(statements[i], sb, "", i == statements.Count - 1);
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

        string childPrefix = prefix + (isLast ? "    " : "│   ");

        var fields = node.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

        for (int i = 0; i < fields.Length; i++)
        {
            var field = fields[i];
            var value = field.GetValue(node);
            if (value == null)
                continue;

            bool fieldIsLast = i == fields.Length - 1;

            // Single AST node
            if (value is ASTnode)
            {
                sb.Append(childPrefix);
                sb.AppendLine(field.Name + ":");
                PrintNode(value, sb, childPrefix, fieldIsLast);
            }
            // List (statements, parameters, etc.)
            else if (value is IEnumerable enumerable && value is not string)
            {
                sb.Append(childPrefix);
                sb.AppendLine(field.Name + ":");

                var items = enumerable.Cast<object>().ToList();
                for (int j = 0; j < items.Count; j++)
                {
                    PrintNode(items[j], sb, childPrefix, j == items.Count - 1);
                }
            }
            // Primitive / enum / token
            else
            {
                sb.Append(childPrefix);
                sb.Append(fieldIsLast ? "└── " : "├── ");
                sb.AppendLine($"{field.Name}: {value}");
            }
        }
    }
}
