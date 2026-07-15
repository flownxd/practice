using System.Text.Json;

namespace task13;

public static class StudentSerializer
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        WriteIndented = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public static string Serialize(Student student)
    {
        if (student == null)
            throw new ArgumentNullException(nameof(student));
        
        return JsonSerializer.Serialize(student, Options);
    }
    
    public static Student Deserialize(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("JSON не может быть пустым", nameof(json));
        
        try
        {
            var student = JsonSerializer.Deserialize<Student>(json, Options);
            ValidateStudent(student);
            return student;
        }
        catch (JsonException ex)
        {
            throw new JsonException($"Ошибка десериализации: {ex.Message}", ex);
        }
    }
    
    public static void SaveToFile(Student student, string filePath)
    {
        if (student == null)
            throw new ArgumentNullException(nameof(student));
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));
        
        string json = Serialize(student);
        File.WriteAllText(filePath, json);
    }
    
    public static Student LoadFromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл не найден: {filePath}");
        
        string json = File.ReadAllText(filePath);
        return Deserialize(json);
    }
    
    private static void ValidateStudent(Student student)
    {
        if (student == null)
            throw new JsonException("Десериализованный объект равен null");
        
        if (string.IsNullOrWhiteSpace(student.FirstName))
            throw new JsonException("FirstName не может быть пустым");
        
        if (string.IsNullOrWhiteSpace(student.LastName))
            throw new JsonException("LastName не может быть пустым");
        
        if (student.BirthDate == default)
            throw new JsonException("BirthDate должен быть установлен");
        
        if (student.BirthDate > DateTime.Now)
            throw new JsonException("BirthDate не может быть в будущем");
    }
}