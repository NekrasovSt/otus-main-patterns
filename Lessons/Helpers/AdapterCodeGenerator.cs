using System.Text;

namespace Lessons.Helpers;

public class AdapterCodeGenerator
{
    private readonly string _interfaceName;
    private readonly string _namespace;
    private readonly Dictionary<string, Type> _getters = new();
    private readonly Dictionary<string, Type> _setters = new();
    private readonly List<string> _methods = new();

    public AdapterCodeGenerator(string interfaceName, string @namespace)
    {
        _interfaceName = interfaceName;
        _namespace = @namespace;
    }

    public AdapterCodeGenerator AddSetter(string name, Type type)
    {
        _setters[name] = type;
        return this;
    }
    
    public AdapterCodeGenerator AddGetter(string name, Type type)
    {
        _getters[name] = type;
        return this;
    }

    public AdapterCodeGenerator AddMethod(string name)
    {
        _methods.Add(name);
        return this;
    }

    public string Build()
    {
        var className = $"{_interfaceName.Remove(0, 1)}Adapter";
        var sb = new StringBuilder();
        sb.AppendLine($"""
                       using Lessons.Infrastructure;
                       """);
        sb.AppendLine($"namespace {_namespace};");
        sb.AppendLine($"public class {className} : {_interfaceName}");
        sb.AppendLine("{");
        sb.AppendLine("\tprivate readonly UObject _universalObject;");
        sb.AppendLine($"\tpublic {className}(UObject universalObject)\n    {{\n        _universalObject = universalObject;\n    }}");


        foreach (var getter in _getters)
        {
            if (_setters.ContainsKey(getter.Key))
            {
                WriteGetterAndSetter(sb, getter.Key, getter.Value);
            }
            else
            {
                WriteGetter(sb, getter.Key, getter.Value);
            }
        }

        foreach (var setter in _setters)
        {
            if (!_getters.ContainsKey(setter.Key))
            {
                WriteSetter(sb, setter.Key, setter.Value);
            }
        }

        foreach (var method in _methods)
        {
            WriteMethod(sb, method);
        }
        sb.AppendLine("}");
        WriteFactory(sb, className, _interfaceName);
        return sb.ToString();
    }

    private void WriteGetter(StringBuilder sb, string name, Type type)
    {
        sb.AppendLine($"\tpublic {type.Name} {name} => Ioc.Resolve<{type.Name}>(\"Spaceship.Operations.{_interfaceName}:{name}.get\", _universalObject);");
    }
    
    private void WriteSetter(StringBuilder sb, string name, Type type)
    {
        sb.AppendLine($"\tpublic {type.Name} {name}\n    {{\n        set =>  Ioc.Resolve<ICommand>(\"Spaceship.Operations.{_interfaceName}:{name}.set\", _universalObject, value).Execute();\n    }}");
    }
    
    private void WriteMethod(StringBuilder sb, string name)
    {
        sb.AppendLine($"\tpublic void {name}()\n    {{\n       Ioc.Resolve<ICommand>(\"Spaceship.Operations.{_interfaceName}:method.{name}\", _universalObject).Execute();\n    }}");
    }

    private void WriteGetterAndSetter(StringBuilder sb, string name, Type type)
    {
        sb.AppendLine($"\tpublic {type} {name}");
        sb.AppendLine("\t{");
        sb.AppendLine($"\t\tget => Ioc.Resolve<{type.Name}>(\"Spaceship.Operations.{_interfaceName}:{name}.get\", _universalObject);");
        sb.AppendLine($"\t\tset => Ioc.Resolve<ICommand>(\"Spaceship.Operations.{_interfaceName}:{name}.set\", _universalObject, value).Execute();");
        sb.AppendLine("\t}");
    }

    private void WriteFactory(StringBuilder sb, string className, string interfaceName)
    {
        sb.AppendLine($"public class {className}Factory : IAdapterFactory");
        sb.AppendLine("{");
        sb.AppendLine("\tpublic object Create(UObject uObject)");
        sb.AppendLine("\t{");
        sb.AppendLine($"\t\treturn new {className}(uObject);");
        sb.AppendLine("\t}");
        sb.AppendLine("\tpublic void RegisterDependencies()");
        sb.AppendLine("\t{");
        foreach (var getter in _getters)
        {
            sb.AppendLine($"\t\tIoc.Resolve<ICommand>(\"IoC.Register\", \"Spaceship.Operations.{interfaceName}:{getter.Key}.get\", (object[] args) => ((UObject)args[0])[\"{getter.Key}\"]).Execute();");
        }
        sb.AppendLine("\t}");
        sb.AppendLine("}");
    }
}