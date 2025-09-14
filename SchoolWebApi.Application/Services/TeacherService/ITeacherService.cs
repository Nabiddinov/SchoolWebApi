using SchoolWebApi.Application.Dtos;

namespace SchoolWebApi.Application.Services.TeacherService;

public interface ITeacherService
{
    Task<IEnumerable<TeacherDto>> GetAllAsync();
    Task<TeacherDto?> GetByIdAsync(int id);
    Task<TeacherDto> CreateAsync(TeacherDto dto);
    Task<TeacherDto?> UpdateAsync(int id, TeacherDto dto);
    Task<bool> DeleteAsync(int id);

    Task<IEnumerable<TeacherDto>> FilterAsync(int? genderId, int? cityId, string? subjectName);
    Task<IEnumerable<TeacherDto>> GetTopTeachersByTopStudentsAsync(bool topHighest, int page, int pageSize);
}