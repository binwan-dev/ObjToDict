namespace ObjToDict;

public class ValueConvertManager
{
    private readonly IValueConverter<DateTime> _dateTimeConverter;

    public ValueConvertManager()
    {
        _dateTimeConverter = new DateTimeValueConverter();
    }

    public string ToString<T>(T data)
    {
        var dateTime = data as DateTime?;
        if (dateTime!=null)
        {
            return _dateTimeConverter.ToString(dateTime.Value);
        }

        return data?.ToString()??string.Empty;
    }

    public T? ToData<T>(string dataString) where T : class
    {
        if (typeof(T) == typeof(DateTime))
        {
            return _dateTimeConverter.ToData(dataString) as T;
        }

        return default(T);
    }
}
