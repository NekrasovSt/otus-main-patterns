using System.Collections;
using System.Reflection;
using Lessons.Infrastructure;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Lessons.Helpers;

public class ReflectionGenerator
{
    private static string Parse<T>(string @namespace)
    {
        if (!typeof(T).IsInterface)
        {
            throw new ArgumentException(nameof(T));
        }
        var adapterCodeGenerator = new AdapterCodeGenerator(typeof(T).Name, @namespace);
        var propertiesWithGet = typeof(T).GetProperties()
            .Where(p => p.CanRead);

        foreach (var propertyInfo in propertiesWithGet)
        {
            adapterCodeGenerator.AddGetter(propertyInfo.Name, propertyInfo.PropertyType);
        }

        var propertiesWithSet = typeof(T).GetProperties()
            .Where(p => p.CanWrite);

        foreach (var propertyInfo in propertiesWithSet)
        {
            adapterCodeGenerator.AddSetter(propertyInfo.Name, propertyInfo.PropertyType);
        }

        foreach (var method in typeof(T).GetMethods().Where(m => !m.Name.StartsWith("set_") && !m.Name.StartsWith("get_")))
        {
            adapterCodeGenerator.AddMethod(method.Name);
        }

        return adapterCodeGenerator.Build();
    }

    private static Assembly Compile(string code)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        // Ссылки на сборки
        var references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // System.Runtime
            MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ICommand).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Collections").Location)
        };

        // Компиляция
        CSharpCompilation compilation = CSharpCompilation.Create(
            "DynamicAssembly",
            [syntaxTree],
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using (var ms = new MemoryStream())
        {
            // Генерация сборки в памяти
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                foreach (Diagnostic error in result.Diagnostics)
                {
                    Console.WriteLine(error.GetMessage());
                }

                return null;
            }

            // Загрузка сборки
            ms.Seek(0, SeekOrigin.Begin);
            return Assembly.Load(ms.ToArray());
        }
    }

    public static IAdapterFactory CreateFactory<T>()
    {
        var @namespace = "Lessons.CodeGen";
        var code = Parse<T>(@namespace);
        var assembly = Compile(code);
        var factoryName = $"{@namespace}.{typeof(T).Name.Remove(0, 1)}AdapterFactory";
        var factoryType = assembly.GetType(factoryName);
        return Activator.CreateInstance(factoryType) as IAdapterFactory;
    }

    public static T CreateInstance<T>(UObject universalObject) where T : class
    {
        var factory = CreateFactory<T>();
        return factory.Create(universalObject) as T;
    }
}