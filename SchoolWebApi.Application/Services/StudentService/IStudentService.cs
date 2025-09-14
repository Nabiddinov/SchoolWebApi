using SchoolWebApi.Application.Dtos;

namespace SchoolWebApi.Application.Services.StudentService;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllAsync();
    Task<StudentDto?> GetByIdAsync(int id);
    Task<StudentDto> CreateAsync(StudentDto dto);
    Task<StudentDto?> UpdateAsync(int id, StudentDto dto);
    Task<bool> DeleteAsync(int id);

    Task<IEnumerable<StudentDto>> FilterAsync(int? genderId, int? departmentId, int? cityId, int? gradeLevel, string? zodiacSign);
    Task<IEnumerable<StudentDto>> GetTopStudentsBySubjectAsync(int subjectId, int top = 10);
}