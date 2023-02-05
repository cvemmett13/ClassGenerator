using System.Text;
using ClassGenerator.Models;
using ClassGenerator.Util;
using Csv;
using NDesk.Options;

var generateParser = false;
var nameOfClass = "Class";
var infile = "";

var p = new OptionSet()
{
    { "p", "whether or not to generate a parsing method", v => generateParser = v != null },
    { "c|className=", "The name of the class to generate.", v => nameOfClass = v },
    { "f|file=", "The path to the file to parse.", v => infile = v },
};

p.Parse(args);

var csv = CsvUtil.Parse(infile);

var cols = ParseFile(csv.ToList());

PrintOutput(generateParser, cols, nameOfClass);

void PrintOutput(bool createParser, List<Column> columns, string className)
{
    var c = BuildClass(columns, className);
    Console.WriteLine(c);

    if (createParser)
    {
        var parser = BuildParser(columns, className);
        Console.WriteLine(parser);
    }
}

List<Column> ParseFile(List<ICsvLine> csvLines)
{
    var columns = new List<Column>();
    int i = 0;
    
    foreach (var header in csvLines[0].Headers)
    {
        columns.Add(new Column(header, TypeUtil.DetermineType(i, csvLines), i));
        i++;
    }
    
    return columns;
}

string BuildClass(List<Column> columns, string className)
{
    var sb = new StringBuilder();

    sb.AppendLine($"public class {className}");
    sb.AppendLine("{");
    foreach (var column in columns)
    {
        sb.AppendLine($"    public {column.Type} {column.Name} {{ get; set; }}");
    }

    sb.AppendLine("}");

    return sb.ToString();
}

string BuildParser(List<Column> columns, string className)
{
    var sb = new StringBuilder();

    sb.AppendLine($"public {className} Parse{className}(List<string> row)");
    sb.AppendLine("{");
    sb.AppendLine($"    {className} record = new {className}();");
    sb.AppendLine("");

    foreach (var column in columns.OrderBy(t => t.Index))
    {
        if (column.Type == "string")
        {
            sb.AppendLine($"    record.{column.Name} = row[{column.Index}];");
        }
        else
        {
            var type = column.Type.Replace("?", "");
            sb.AppendLine($"    record.{column.Name} = {type}.Parse(row[{column.Index}]);");
        }
    }

    sb.AppendLine("");
    sb.AppendLine("    return record;");
    sb.AppendLine("}");

    return sb.ToString();
}