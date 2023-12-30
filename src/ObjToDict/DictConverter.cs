namespace ObjToDict;

public class DictConverter
{
    private static Dictionary<string,TypeMetadata> _metadatas = new();
    private static CodeDescripter _codeDescripter = new();

    public static Dictionary<string, string>? ToDictAsync<T>(T data)
    {
	var name = typeof(T).FullName;
        if(!_metadatas.TryGetValue(typeof(T).FullName??"",out var metadata))
        {
            var conveter=_codeDescripter.CreateAsync<T>().Result;
            metadata = new TypeMetadata()
            {
                FullName = typeof(T).FullName ?? string.Empty,
                Type = typeof(T),
                DictConverter = conveter
            };
	    _metadatas.Add(typeof(T).FullName??string.Empty, metadata);
        }

	// var converter=metadata.DictConverter as IDictConverter<T>;
        // if (converter == null)
        // {
        //     return null;
        // }

        return metadata.DictConverter.ToDict(data);
    }
}
