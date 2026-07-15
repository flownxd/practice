using System.Text.Json.Serialization;

namespace task13;

public class Student
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    [JsonConverter(typeof(JsonDateTimeConverter))]
    public DateTime BirthDate { get; set; }
    
    public List<Subject> Grades { get; set; }
    
    [JsonIgnore]
    public string FullName => $"{FirstName} {LastName}";
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Email { get; set; }
}