namespace ObjToDict;

public class DateTimeValueConverter : IValueConverter<DateTime>
{
    public DateTime ToData(string dataString)
    {
        if (string.IsNullOrWhiteSpace(dataString))
        {
            return DateTime.MinValue;
        }

        return DateTime.Parse(dataString);
    }

    public string ToString(DateTime data) => data.ToString("yyyy/MM/dd HH:mm:ss");
}
