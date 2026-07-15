using Xunit;
using System.Text.Json;
using task13;

namespace task13tests;

public class StudentSerializerTests
{
    private Student GetTestStudent()
    {
        return new Student
        {
            FirstName = "Иван",
            LastName = "Петров",
            BirthDate = new DateTime(2000, 5, 15),
            Grades = new List<Subject>
            {
                new Subject { Name = "Математика", Grade = 5 },
                new Subject { Name = "Физика", Grade = 4 }
            }
        };
    }
    
    [Fact]
    public void Serialize_ShouldReturnValidJson()
    {
    var student = GetTestStudent();
    string json = StudentSerializer.Serialize(student);
    
    Assert.NotNull(json);
    Assert.Contains("firstName", json);
    Assert.Contains("lastName", json);
    Assert.Contains("birthDate", json);
    Assert.Contains("grades", json);
    }
    
    [Fact]
    public void Serialize_ShouldIgnoreNullEmail()
    {
        var student = GetTestStudent();
        student.Email = null;
        string json = StudentSerializer.Serialize(student);
        
        Assert.DoesNotContain("email", json);
    }
    
    [Fact]
    public void Serialize_ShouldIgnoreFullNameProperty()
    {
        var student = GetTestStudent();
        string json = StudentSerializer.Serialize(student);
        
        Assert.DoesNotContain("FullName", json);
        Assert.DoesNotContain("full_name", json);
    }
    
    [Fact]
    public void Serialize_ShouldFormatDateCorrectly()
    {
        var student = GetTestStudent();
        string json = StudentSerializer.Serialize(student);
        
        Assert.Contains("2000-05-15", json);
    }
    
    [Fact]
    public void Deserialize_ShouldCreateValidStudent()
    {
    string json = @"{
        ""firstName"": ""Анна"",
        ""lastName"": ""Сидорова"",
        ""birthDate"": ""1999-10-20"",
        ""grades"": [
            {""name"": ""Химия"", ""grade"": 5}
        ]
    }";
    
    var student = StudentSerializer.Deserialize(json);
    
    Assert.Equal("Анна", student.FirstName);
    Assert.Equal("Сидорова", student.LastName);
    Assert.Equal(new DateTime(1999, 10, 20), student.BirthDate);
    Assert.Single(student.Grades);
    Assert.Equal("Химия", student.Grades[0].Name);
    }
    
    [Fact]
    public void Deserialize_ShouldThrowOnEmptyJson()
    {
        Assert.Throws<ArgumentException>(() => StudentSerializer.Deserialize(""));
    }
    
    [Fact]
    public void Deserialize_ShouldThrowOnInvalidJson()
    {
        string invalidJson = "{ invalid json }";
        Assert.Throws<JsonException>(() => StudentSerializer.Deserialize(invalidJson));
    }
    
    [Fact]
    public void Deserialize_ShouldThrowOnMissingFirstName()
    {
    string json = @"{
        ""lastName"": ""Петров"",
        ""birthDate"": ""2000-01-01"",
        ""grades"": []
    }";
    
    Assert.Throws<JsonException>(() => StudentSerializer.Deserialize(json));
    }
    
    [Fact]
    public void Deserialize_ShouldThrowOnFutureBirthDate()
    {
    string json = @"{
        ""firstName"": ""Иван"",
        ""lastName"": ""Петров"",
        ""birthDate"": ""2099-01-01"",
        ""grades"": []
    }";
    
    Assert.Throws<JsonException>(() => StudentSerializer.Deserialize(json));
    }
    
    [Fact]
    public void SaveToFile_And_LoadFromFile_ShouldWorkCorrectly()
    {
        var student = GetTestStudent();
        string filePath = Path.Combine(Path.GetTempPath(), $"test_student_{Guid.NewGuid()}.json");
        
        try
        {
            StudentSerializer.SaveToFile(student, filePath);
            Assert.True(File.Exists(filePath));
            
            var loaded = StudentSerializer.LoadFromFile(filePath);
            
            Assert.Equal(student.FirstName, loaded.FirstName);
            Assert.Equal(student.LastName, loaded.LastName);
            Assert.Equal(student.BirthDate, loaded.BirthDate);
            Assert.Equal(student.Grades.Count, loaded.Grades.Count);
        }
        finally
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
    
    [Fact]
    public void SaveToFile_ShouldThrowOnNullStudent()
    {
        Assert.Throws<ArgumentNullException>(() => 
            StudentSerializer.SaveToFile(null, "test.json"));
    }
    
    [Fact]
    public void LoadFromFile_ShouldThrowOnNonExistentFile()
    {
        Assert.Throws<FileNotFoundException>(() => 
            StudentSerializer.LoadFromFile("non_existent_file.json"));
    }
    
    [Fact]
    public void Serialize_ShouldUseCamelCaseNaming()
    {
        var student = GetTestStudent();
        string json = StudentSerializer.Serialize(student);
        
        Assert.Contains("firstName", json);
        Assert.Contains("lastName", json);
    }
}