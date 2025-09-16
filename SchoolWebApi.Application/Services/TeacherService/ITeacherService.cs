using SchoolWebApi.Application.Dtos;
using SchoolWebApi.Application.Dtos.TeacherDTOs;

namespace SchoolWebApi.Application.Services.TeacherService;

public interface ITeacherService
{
    Task<IEnumerable<TeacherDto>> GetAllAsync();
    Task<TeacherDto?> GetByIdAsync(int id);
    Task<TeacherDto> CreateAsync(TeacherCreateDto dto);
    Task<TeacherDto?> UpdateAsync(int id, TeacherDto dto);
    Task<bool> DeleteAsync(int id);
  
    Task<IEnumerable<TeacherDto>>GetTeachersByTopStudentsAsync(bool topHighest, int topStudents, int topTeachers);
    Task<IEnumerable<TeacherDto>> FilterAsync(int? genderId, int? cityId, string? subjectName);
}