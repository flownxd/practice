using System.Reflection;

if (args.Length == 0)
{
    Console.WriteLine("Использование: MetadataViewer <path_to_dll>");
    return;
}

string dllPath = args[0];

if (!File.Exists(dllPath))
{
    Console.WriteLine($"Ошибка: файл '{dllPath}' не найден");
    return;
}

try
{
    Assembly assembly = Assembly.LoadFrom(dllPath);
    
    Console.WriteLine($"=== Метаданные сборки: {assembly.GetName().Name} ===\n");
    
    Type[] types = assembly.GetTypes();
    
    foreach (Type type in types)
    {
        if (type.IsClass && !type.IsAbstract)
        {
            Console.WriteLine($"Класс: {type.Name}");
            
            var displayNameAttr = type.GetCustomAttribute(typeof(CommandLib.DisplayNameAttribute)) as CommandLib.DisplayNameAttribute;
            if (displayNameAttr != null)
            {
                Console.WriteLine($"  DisplayName: {displayNameAttr.DisplayName}");
            }
            
            var versionAttr = type.GetCustomAttribute(typeof(CommandLib.VersionAttribute)) as CommandLib.VersionAttribute;
            if (versionAttr != null)
            {
                Console.WriteLine($"  Version: {versionAttr.Major}.{versionAttr.Minor}");
            }
            
            Console.WriteLine("  Атрибуты:");
            var attributes = type.GetCustomAttributes(false);
            foreach (var attr in attributes)
            {
                Console.WriteLine($"    - {attr.GetType().Name}");
            }
            
            Console.WriteLine("  Конструкторы:");
            var constructors = type.GetConstructors();
            foreach (var ctor in constructors)
            {
                var parameters = ctor.GetParameters();
                if (parameters.Length == 0)
                {
                    Console.WriteLine($"    - {type.Name}()");
                }
                else
                {
                    string paramList = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
                    Console.WriteLine($"    - {type.Name}({paramList})");
                }
                
                foreach (var param in parameters)
                {
                    Console.WriteLine($"      Параметр: {param.ParameterType.Name} {param.Name}");
                }
            }
            
            Console.WriteLine("  Методы:");
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                var methodDisplayName = method.GetCustomAttribute(typeof(CommandLib.DisplayNameAttribute)) as CommandLib.DisplayNameAttribute;
                string displayName = methodDisplayName != null ? $" [{methodDisplayName.DisplayName}]" : "";
                
                var parameters = method.GetParameters();
                string paramList = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
                Console.WriteLine($"    - {method.ReturnType.Name} {method.Name}({paramList}){displayName}");
                
                foreach (var param in parameters)
                {
                    Console.WriteLine($"      Параметр: {param.ParameterType.Name} {param.Name}");
                }
            }
            
            Console.WriteLine("  Свойства:");
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var prop in properties)
            {
                Console.WriteLine($"    - {prop.PropertyType.Name} {prop.Name}");
            }
            
            Console.WriteLine();
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка при загрузке сборки: {ex.Message}");
}