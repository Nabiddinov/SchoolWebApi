namespace SchoolWebApi.Application.Dtos.StudentDTOs;

public class StudentUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public int GenderId { get; set; }
    public int CityId { get; set; }
    public int DepartmentId { get; set; }
    public int CurrentGradeLevel { get; set; }
    public DateTime BirthDate { get; set; }
}