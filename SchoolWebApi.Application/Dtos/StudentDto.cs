namespace SchoolWebApi.Application.Dtos;

public record StudentDto(
    int Id,
    string Name,
    string Gender,
    string CityName,
    string DepartmentName,
    int CurrentGradeLevel,
    DateTime BirthDate
);