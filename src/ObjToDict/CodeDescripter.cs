using GenAssembly;
using GenAssembly.Descripters;
using System.Reflection;

namespace ObjToDict;

public class CodeDescripter
{
    private readonly string _namespace = "ObjToDict.CodeGeneration";
    private readonly CodeBuilder _codeBuilder;

    public CodeDescripter()
    {
	_codeBuilder=new CodeBuilder(_namespace, _namespace);
    }

    public async Task<IDictConverter?> CreateAsync<T>()
    {
	_codeBuilder.AddAssemblyRefence(typeof(T).Assembly);
	_codeBuilder.AddAssemblyRefence(typeof(IDictConverter).Assembly);
	
        var properties = typeof(T).GetProperties(BindingFlags.Public|BindingFlags.Instance);

	var className = $"{typeof(T).Name}Converter";
        var classDescripter = new ClassDescripter(className, _namespace)
	    .SetAccess(AccessType.Public)
            .SetBaseType(typeof(T).FullName, $"IDictConverter<{typeof(T).FullName}>");
	classDescripter.Methods.Add(GenerateToDictMethod<T>(classDescripter,properties));
        _codeBuilder.CreateClass(classDescripter);
	var assembly= await _codeBuilder.BuildAsync();
	
	return assembly.GetObj(className,_namespace) as IDictConverter;
    }

    private MethodDescripter GenerateToDictMethod<T>(ClassDescripter classDescripter,PropertyInfo[] properties)
    {
	var toDictMethod = new MethodDescripter("ToDict", classDescripter)
	    .SetAccess(AccessType.Public)
            .SetReturnType("Dictionary<string, string>");
        toDictMethod.Parameters.Add(new ParameterDescripter(typeof(T).FullName, "data"));
        toDictMethod.AppendCode($@"	var dict = new Dictionary<string, string>();");
        foreach (var property in properties)
        {
	    toDictMethod.AppendCode(@$"            dict.Add(""{property.Name}"", data.{property.Name}.ToString());");
	}
	toDictMethod.AppendCode("            return dict;");
	return toDictMethod;
    }

}
