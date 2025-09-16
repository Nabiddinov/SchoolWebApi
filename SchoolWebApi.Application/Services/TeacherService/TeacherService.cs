using SchoolWebApi.Domain.Entities;
using SchoolWebApi.Infrastructure.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolWebApi.Application.Dtos.TeacherDTOs;

namespace SchoolWebApi.Application.Services.TeacherService;

public class TeacherService : ITeacherService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TeacherService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TeacherDto>> GetAllAsync()
    {
        return await _context.Teachers
            .Include(t => t.City)
            .Include(t => t.TeacherSubjects)
                .ThenInclude(ts => ts.Subject)
            .ProjectTo<TeacherDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<TeacherDto?> GetByIdAsync(int id)
    {
        return await _context.Teachers
            .Include(t => t.City)
            .Include(t => t.TeacherSubjects)
                .ThenInclude(ts => ts.Subject)
            .Where(t => t.Id == id)
            .ProjectTo<TeacherDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }
    public async Task<TeacherDto> CreateAsync(TeacherCreateDto dto)
    {
        // 1) City tekshiruv
        var cityExists = await _context.Cities.AnyAsync(c => c.Id == dto.CityId);
        if (!cityExists)
            throw new Exception($"CityId {dto.CityId} mavjud emas!");

        // 2) Subjectlar tekshiruv
        var validSubjects = await _context.Subjects
            .Where(s => dto.SubjectIds.Contains(s.Id))
            .Select(s => s.Id)
            .ToListAsync();

        if (validSubjects.Count != dto.SubjectIds.Distinct().Count())
            throw new Exception("SubjectIds dan ba'zilari mavjud emas yoki takrorlangan!");

        // 3) Teacher yaratish
        var teacher = _mapper.Map<Teacher>(dto);
        teacher.CreatedDate = DateTime.UtcNow;
        teacher.LastUpdatedDate = DateTime.UtcNow;

        // 4) Subject bog‘lash (duplicate oldini olish uchun Distinct())
        teacher.TeacherSubjects = dto.SubjectIds
            .Distinct()
            .Select(subjectId => new TeacherSubject
            {
                SubjectId = subjectId,
                Teacher = teacher
            }).ToList();

        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();

        return _mapper.Map<TeacherDto>(teacher);
    }

    public async Task<TeacherDto?> UpdateAsync(int id, TeacherDto dto)
    {
        var entity = await _context.Teachers.FindAsync(id);
        if (entity == null) return null;
        
        dto.Id = id;
        
        _mapper.Map(dto, entity);
        entity.LastUpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Teachers
            .Include(t => t.TeacherSubjects) 
            .FirstOrDefaultAsync(t => t.Id == id);
            
        if (entity == null) return false;

        _context.Teachers.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<TeacherDto>> FilterAsync(int? genderId, int? cityId, string? subjectName)
    {
        var query = _context.Teachers
            .Include(t => t.City)
            .Include(t => t.TeacherSubjects)
                .ThenInclude(ts => ts.Subject)
            .AsQueryable();

        if (genderId.HasValue)
            query = query.Where(t => t.GenderId == genderId);
            
        if (cityId.HasValue)
            query = query.Where(t => t.CityId == cityId);
            
        if (!string.IsNullOrEmpty(subjectName))
            query = query.Where(t => t.TeacherSubjects
                .Any(ts => ts.Subject.Name.Contains(subjectName))); 

        return await query
            .ProjectTo<TeacherDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<IEnumerable<TeacherDto>> GetTopTeachersByTopStudentsAsync(bool topHighest, int page = 1, int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100; 

        var topStudentIds = await _context.StudentSubjects
            .GroupBy(ss => ss.StudentId)
            .Select(g => new 
            { 
                StudentId = g.Key, 
                AverageMark = g.Average(ss => ss.Mark) 
            })
            .OrderBy(x => topHighest ? -x.AverageMark : x.AverageMark) // topHighest bo'yicha tartiblash
            .Take(5)
            .Select(x => x.StudentId)
            .ToListAsync();

        var teacherQuery = _context.Teachers
            .Include(t => t.City)
            .Include(t => t.TeacherSubjects)
                .ThenInclude(ts => ts.Subject)
            .Where(t => t.TeacherSubjects.Any(ts =>
                ts.Subject.StudentSubjects.Any(ss => topStudentIds.Contains(ss.StudentId))))
            .Distinct(); 

        return await teacherQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<TeacherDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Teachers.CountAsync();
    }

    public async Task<IEnumerable<TeacherDto>> GetPagedAsync(int page = 1, int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        return await _context.Teachers
            .Include(t => t.City)
            .Include(t => t.TeacherSubjects)
                .ThenInclude(ts => ts.Subject)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<TeacherDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Teachers.AnyAsync(t => t.Id == id);
    }
    
    public async Task<IEnumerable<TeacherDto>> GetTeachersByTopStudentsAsync(bool topHighest, int topStudents = 5, int topTeachers = 10)
    {
        // 1) TOP yoki LOW studentlarni aniqlash
        var topStudentIds = await _context.StudentSubjects
            .GroupBy(ss => ss.StudentId)
            .Select(g => new 
            { 
                StudentId = g.Key, 
                AverageMark = g.Average(ss => ss.Mark) 
            })
            .OrderBy(x => topHighest ? -x.AverageMark : x.AverageMark)
            .Take(topStudents)
            .Select(x => x.StudentId)
            .ToListAsync();

        // 2) Shu studentlarga dars berayotgan teacherlarni olish
        var teacherQuery = _context.Teachers
            .Include(t => t.City)
            .Include(t => t.TeacherSubjects)
            .ThenInclude(ts => ts.Subject)
            .Where(t => t.TeacherSubjects.Any(ts =>
                ts.Subject.StudentSubjects.Any(ss => topStudentIds.Contains(ss.StudentId))))
            .Distinct();

        return await teacherQuery
            .ProjectTo<TeacherDto>(_mapper.ConfigurationProvider)
            .Take(topTeachers)
            .ToListAsync();
    }
}