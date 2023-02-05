using Csv;

namespace ClassGenerator.Util;

public class TypeUtil
{
    public static string DetermineType(int colIndex, IEnumerable<ICsvLine> csv)
    {
        bool couldBeDate = true;
        bool couldBeFloat = true;
        bool couldBeInt = true;
        bool couldBeBool = true;
        bool nullable = false;

        foreach (var line in csv)
        {
            var value = line[colIndex];
            if (String.IsNullOrEmpty(value))
            {
                nullable = true;
                continue;
            }

            if (!DateTime.TryParse(value, out _)) couldBeDate = false;
            if (!float.TryParse(value, out _)) couldBeFloat = false;
            if (!int.TryParse(value, out _)) couldBeInt = false;
            if (!bool.TryParse(value, out _)) couldBeBool = false;
        }

        if (couldBeInt) return nullable ? "int?" : "int";
        if (couldBeFloat) return nullable ? "float?" : "float";
        if (couldBeBool) return nullable ? "bool?" : "bool";
        if (couldBeDate) return nullable ? "DateTime?" : "DateTime";

        return "string";
    }
}