namespace SchoolWebApi.Application.Dtos.TeacherDTOs;

public class TeacherCreateDto
{
    public string Name { get; set; } = null!;
    public int CityId { get; set; }
    public DateTime BirthDate { get; set; }
    public int GenderId { get; set; }
    public List<int> SubjectIds { get; set; } = new();
}