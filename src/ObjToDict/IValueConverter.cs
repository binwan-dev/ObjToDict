namespace ObjToDict;

public interface IValueConverter<T>
{
    string ToString(T data);

    T ToData(string dataString);
}
