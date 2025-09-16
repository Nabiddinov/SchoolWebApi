namespace SchoolWebApi.Application.Dtos.TeacherDTOs;

public class TeacherDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public string CityName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public List<string> Subjects { get; set; } = new();
}
