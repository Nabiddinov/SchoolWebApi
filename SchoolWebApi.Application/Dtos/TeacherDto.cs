namespace SchoolWebApi.Application.Dtos;

public record TeacherDto(
    int Id,
    string Name,
    string Gender,
    string CityName,
    DateTime BirthDate,
    List<string> Subjects
);