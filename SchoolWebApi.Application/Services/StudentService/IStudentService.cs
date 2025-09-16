using SchoolWebApi.Application.Dtos;
using SchoolWebApi.Application.Dtos.StudentDTOs;

namespace SchoolWebApi.Application.Services.StudentService;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllAsync();
    Task<StudentDto?> GetByIdAsync(int id);
    Task<StudentDto> CreateAsync(StudentCreateDto dto);
    Task<StudentDto?> UpdateAsync(int id, StudentUpdateDto dto);
    Task<bool> DeleteAsync(int id);

    Task<IEnumerable<StudentDto>> GetTopStudentsBySubjectOrderedAsync(int subjectId, bool topHighest, int top);
    Task<IEnumerable<StudentDto>> FilterAsync(int? genderId, int? departmentId, int? cityId, int? gradeLevel, string? zodiacSign);
}