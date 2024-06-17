namespace ObjToDict;

public class DictConverter
{
    private static Dictionary<string,TypeMetadata> _metadatas = new();
    private static CodeDescripter _codeDescripter = new();

    public static Dictionary<string, string>? ToDictAsync<T>(T data)
    {
        var ttype = typeof(T);
        if (!_metadatas.TryGetValue(ttype.FullName ?? "", out var metadata))
        {
            var conveter = _codeDescripter.CreateAsync<T>().Result;
            metadata = new TypeMetadata()
            {
                FullName = ttype.FullName ?? string.Empty,
                Type = ttype,
                DictConverter = conveter ?? throw new InvalidOperationException($"Cannot create converter for {ttype.FullName}")
            };
            _metadatas.Add(ttype.FullName ?? string.Empty, metadata);
        }

        var converter = metadata.DictConverter as IDictConverter<T>;
        if (converter == null) return null;

        return converter.ToDict(data);
    }
}
