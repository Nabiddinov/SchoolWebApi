namespace SchoolWebApi.Application.Dtos.StudentDTOs;

public class StudentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public int CurrentGradeLevel { get; set; }
    public DateTime BirthDate { get; set; }
    public StudentDto() { }
}