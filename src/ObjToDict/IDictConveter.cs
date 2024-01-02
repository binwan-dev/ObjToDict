namespace ObjToDict;

public interface IDictConverter
{
}

public interface IDictConverter<T>:IDictConverter
{
    Dictionary<string, string> ToDict(T data);
}
