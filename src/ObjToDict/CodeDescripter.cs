using GenAssembly;
using GenAssembly.Descripters;
using System.Reflection;

namespace ObjToDict;

public class CodeDescripter
{
    private readonly string _namespace = "ObjToDict.CodeGeneration";
    private readonly CodeBuilder _codeBuilder;
    private static Assembly _dictConverterAssembly;

    static CodeDescripter()
    {
        _dictConverterAssembly = typeof(IDictConverter).Assembly;
    }

    public CodeDescripter()
    {
	    _codeBuilder=new CodeBuilder(_namespace, _namespace);
        _codeBuilder.AddAssemblyRefence(_dictConverterAssembly);
    }

    public async Task<IDictConverter?> CreateAsync<T>()
    {
        var tType = typeof(T);
	    _codeBuilder.AddAssemblyRefence(tType.Assembly);
	
        var properties = tType.GetProperties(BindingFlags.Public|BindingFlags.Instance);
        var isRecord = ((TypeInfo)tType).DeclaredProperties.Any(x => x.Name=="EqualityContract");

        var className = $"{tType.Name}Converter";
        var classDescripter = new ClassDescripter(className, _namespace)
	        .SetAccess(AccessType.Public)
            .SetBaseType(tType.FullName, $"IDictConverter<{tType.FullName}>")
	        .CreateFiled(new FieldDescripter("_valueConvertManager")
			.SetType(typeof(ValueConvertManager))
			.SetRightCode("new ValueConvertManager()")
			.SetAccess(AccessType.PrivateReadonly));
        if (isRecord) classDescripter.SetRecord();
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
        toDictMethod.AppendCode($@"var dict = new Dictionary<string, string>();");
        foreach (var property in properties)
        {
	    toDictMethod.AppendCode(@$"            dict.Add(""{property.Name}"", _valueConvertManager.ToString(data.{property.Name}));");
	}
	toDictMethod.AppendCode("            return dict;");
	return toDictMethod;
    }

}
