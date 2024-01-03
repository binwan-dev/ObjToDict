using System.Diagnostics;
using ObjToDict;

var stopWatch = new Stopwatch();
var person = new Person(){Name="Bin",Age=29};
stopWatch.Start();
for(var i =0;i<10000000;i++)
{
    person.ToDict();
}
stopWatch.Stop();
Console.WriteLine($"Normal TotalSpeedTime: {stopWatch.ElapsedMilliseconds}");

DictConverter.ToDictAsync<Person>(person);
stopWatch.Restart();
for(var i =0;i<10000000;i++)
{
    DictConverter.ToDictAsync<Person>(person);
}
stopWatch.Stop();
Console.WriteLine($"ObjToDict TotalSpeedTime: {stopWatch.ElapsedMilliseconds}");
var s = DictConverter.ToDictAsync<Person>(person);
if(s==null)
{
    Console.WriteLine("null dict");
    return;
}

foreach(var item in s)
{
    Console.WriteLine($"{item.Key}: {item.Value}");
}

Console.WriteLine("Hello, World!");

public class Person
{
    public string Name { get; set; } = string.Empty;

    public int Age{ get;set; }

    public DateTime Birthday{ get;set; }

    public Dictionary<string,string> ToDict()
    {
	return new Dictionary<string,string>();
    }
}
