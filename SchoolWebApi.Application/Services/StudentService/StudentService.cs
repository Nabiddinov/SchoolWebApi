using SchoolWebApi.Application.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolWebApi.Application.Dtos.StudentDTOs;
using SchoolWebApi.Domain.Entities;
using SchoolWebApi.Infrastructure.Data;

namespace SchoolWebApi.Application.Services.StudentService;

public class StudentService : IStudentService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public StudentService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StudentDto>> GetAllAsync()
    {
        return await _context.Students
            .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<StudentDto?> GetByIdAsync(int id)
    {
        return await _context.Students
            .Where(s => s.Id == id)
            .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public async Task<StudentDto> CreateAsync(StudentCreateDto dto)
    {
        var entity = _mapper.Map<Student>(dto);
        entity.CreatedDate = DateTime.UtcNow;
        
        _context.Students.Add(entity);
        await _context.SaveChangesAsync();
        
        var createdStudent = await _context.Students
            .Where(s => s.Id == entity.Id)
            .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
            
        return createdStudent!;
    }

    public async Task<StudentDto?> UpdateAsync(int id, StudentUpdateDto dto)
    {
        var entity = await _context.Students.FindAsync(id);
        if (entity == null) return null;

        _mapper.Map(dto, entity);
        entity.LastUpdatedDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        
        var updatedStudent = await _context.Students
            .Where(s => s.Id == id)
            .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
            
        return updatedStudent;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Students.FindAsync(id);
        if (entity == null) return false;

        _context.Students.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<StudentDto>> FilterAsync(int? genderId, int? departmentId, int? cityId, int? gradeLevel, string? zodiacSign)
    {
        var query = _context.Students.AsQueryable();

        if (genderId.HasValue)
            query = query.Where(s => s.GenderId == genderId.Value);
        if (departmentId.HasValue)
            query = query.Where(s => s.DepartmentId == departmentId);
        if (cityId.HasValue)
            query = query.Where(s => s.CityId == cityId);
        if (gradeLevel.HasValue)
            query = query.Where(s => s.CurrentGradeLevel == gradeLevel);

        return await query.ProjectTo<StudentDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<IEnumerable<StudentDto>> GetTopStudentsBySubjectAsync(int subjectId, int top = 10)
    {
        return await _context.StudentSubjects
            .Where(ss => ss.SubjectId == subjectId)
            .OrderByDescending(ss => ss.Mark)
            .ThenByDescending(ss => ss.Student.CurrentGradeLevel)
            .ThenBy(ss => ss.Student.BirthDate)
            .Select(ss => ss.Student)
            .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
            .Take(top)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<StudentDto>> GetTopStudentsBySubjectOrderedAsync(int subjectId, bool topHighest = true, int top = 10)
    {
        var query = _context.StudentSubjects
            .Where(ss => ss.SubjectId == subjectId);

        // tartiblash: Mark → GradeLevel → BirthDate
        query = topHighest
            ? query.OrderByDescending(ss => ss.Mark)
                .ThenByDescending(ss => ss.Student.CurrentGradeLevel)
                .ThenBy(ss => ss.Student.BirthDate)
            : query.OrderBy(ss => ss.Mark)
                .ThenBy(ss => ss.Student.CurrentGradeLevel)
                .ThenByDescending(ss => ss.Student.BirthDate);

        return await query
            .Select(ss => ss.Student)
            .Distinct()
            .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
            .Take(top)
            .ToListAsync();
    }
}