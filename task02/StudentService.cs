namespace task02;

public class StudentService
{
    private readonly List<Student> std;

    public StudentService(List<Student> students)
    {
        std = students;
    }

    public IEnumerable<Student> GetStudentsByFaculty(string faculty)
    {
        return std
            .Where(student => student.Faculty.Equals(faculty));
    }

    public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade)
    {
        return std
            .Where(s => CalculateAverage(s.Grades) >= minAverageGrade);
    }

    public IEnumerable<Student> GetStudentsOrderedByName()
    {
        return std
            .OrderBy(student => student.Name);
    }

    public ILookup<string, Student> GroupStudentsByFaculty()
    {
        return std
            .ToLookup(student => student.Faculty);
    }

    public string GetFacultyWithHighestAverageGrade()
    {
        var facultygroups = std
            .GroupBy(student => student.Faculty);

        var bestfaculty = facultygroups
            .OrderByDescending(group => group.Average(student => student.Grades.Average()))
            .First();

        return bestfaculty.Key;
    }

    private double CalculateAverage(List<int> grades)
    {
        return grades.Average();
    }
}