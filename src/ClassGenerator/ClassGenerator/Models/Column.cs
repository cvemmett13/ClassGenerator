namespace ClassGenerator.Models;

public class Column
{
    public Column(string name, string type, int index)
    {
        Name = name;
        Type = type;
        Index = index;
    }

    public string Name { get; set; }
    public string Type { get; set; }
    public int Index { get; set; }
}