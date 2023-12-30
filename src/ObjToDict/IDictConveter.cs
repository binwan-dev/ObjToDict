namespace ObjToDict;

public interface IDictConverter
{
    Dictionary<string,string> ToDict(object data);
}

public interface IDictConverter<T>:IDictConverter
{
    // Dictionary<string, string> ToDict(T data);
}
