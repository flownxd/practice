using task13;

Console.WriteLine("Сериализация/Десериализация Student\n");

var student = new Student
{
    FirstName = "Иван",
    LastName = "Петров",
    BirthDate = new DateTime(2000, 5, 15),
    Grades = new List<Subject>
    {
        new Subject { Name = "Математика", Grade = 5 },
        new Subject { Name = "Физика", Grade = 4 },
        new Subject { Name = "Информатика", Grade = 5 }
    },
    Email = null
};

Console.WriteLine("Оригинальный объект:");
Console.WriteLine($"Имя: {student.FullName}");
Console.WriteLine($"Дата рождения: {student.BirthDate:dd.MM.yyyy}");
Console.WriteLine($"Предметов: {student.Grades.Count}\n");

string json = StudentSerializer.Serialize(student);
Console.WriteLine("JSON (сериализация):");
Console.WriteLine(json);
Console.WriteLine();

try
{
    Student deserialized = StudentSerializer.Deserialize(json);
    Console.WriteLine("Десериализация успешна!");
    Console.WriteLine($"Имя: {deserialized.FullName}");
    Console.WriteLine($"Дата рождения: {deserialized.BirthDate:dd.MM.yyyy}\n");
    
    string filePath = "student.json";
    StudentSerializer.SaveToFile(student, filePath);
    Console.WriteLine($"Данные сохранены в файл: {filePath}");
    
    Student loaded = StudentSerializer.LoadFromFile(filePath);
    Console.WriteLine($"Данные загружены из файла: {filePath}");
    Console.WriteLine($"Имя: {loaded.FullName}");
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}