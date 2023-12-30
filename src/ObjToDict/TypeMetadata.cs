namespace ObjToDict;

public class TypeMetadata
{
    public string FullName { get; set; } = string.Empty;

    public Type Type { get; set; } = null!;

    public IDictConverter DictConverter { get; set; } = null!;
}
