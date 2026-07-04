using System.Reflection;
using System.Text;

namespace task07;

public static class ReflectionHelper
{
    public static string PrintTypeInfo(Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type), "Невозможно проанализировать null-тип");
        
        StringBuilder output = new StringBuilder();
        output.AppendLine($"Анализируем тип: {type.Name}");

        DisplayNameAttribute displayNameAttr = type.GetCustomAttribute<DisplayNameAttribute>();
        if (displayNameAttr != null)
        {
            output.AppendLine($"Отображаемое имя: {displayNameAttr.DisplayName}");
        }

        VersionAttribute versionAttr = type.GetCustomAttribute<VersionAttribute>();
        if (versionAttr != null)
        {
            output.AppendLine($"Версия: {versionAttr.Major}.{versionAttr.Minor}");
        }

        var allMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
        foreach (var method in allMethods)
        {
            DisplayNameAttribute methodAttr = method.GetCustomAttribute<DisplayNameAttribute>();
            if (methodAttr != null)
            {
                output.AppendLine($"Метод {method.Name}: {methodAttr.DisplayName}");
            }
        }

        var allProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
        foreach (var prop in allProperties)
        {
            DisplayNameAttribute propAttr = prop.GetCustomAttribute<DisplayNameAttribute>();
            if (propAttr != null)
            {
                output.AppendLine($"Свойство {prop.Name}: {propAttr.DisplayName}");
            }
        }

        return output.ToString();
    }
}