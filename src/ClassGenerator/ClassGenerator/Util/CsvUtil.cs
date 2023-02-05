using Csv;

namespace ClassGenerator.Util;

public class CsvUtil
{
    public static IEnumerable<ICsvLine> Parse(string filePath)
    {
        var text = File.ReadAllText(filePath);
        
        var csv = CsvReader.ReadFromText(text);

        return csv;
    }
}