using SchoolWebApi.Domain.Entities;
using SchoolWebApi.Application.Dtos;
using SchoolWebApi.Infrastructure.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

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
                .ProjectTo<TeacherDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TeacherDto?> GetByIdAsync(int id)
        {
            return await _context.Teachers
                .Where(t => t.Id == id)
                .ProjectTo<TeacherDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<TeacherDto> CreateAsync(TeacherDto dto)
        {
            var entity = _mapper.Map<Teacher>(dto);
            entity.CreatedDate = DateTime.UtcNow;
            _context.Teachers.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TeacherDto>(entity);
        }

        public async Task<TeacherDto?> UpdateAsync(int id, TeacherDto dto)
        {
            var entity = await _context.Teachers.FindAsync(id);
            if (entity == null) return null;

            _mapper.Map(dto, entity);
            entity.LastUpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return _mapper.Map<TeacherDto>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Teachers.FindAsync(id);
            if (entity == null) return false;

            _context.Teachers.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TeacherDto>> FilterAsync(int? genderId, int? cityId, string? subjectName)
        {
            var query = _context.Teachers.AsQueryable();

            if (genderId.HasValue)
                query = query.Where(t => t.GenderId == genderId);
            if (cityId.HasValue)
                query = query.Where(t => t.CityId == cityId);
            if (!string.IsNullOrEmpty(subjectName))
                query = query.Where(t => t.TeacherSubjects.Any(ts => ts.Subject.Name == subjectName));

            return await query.ProjectTo<TeacherDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<TeacherDto>> GetTopTeachersByTopStudentsAsync(bool topHighest, int page, int pageSize)
        {
            var studentQuery = _context.StudentsSubjects.AsQueryable();

            studentQuery = topHighest
                ? studentQuery.OrderByDescending(ss => ss.Mark)
                : studentQuery.OrderBy(ss => ss.Mark);

            var topStudents = studentQuery
                .Select(ss => ss.StudentId)
                .Distinct()
                .Take(5)
                .ToList();

            var teacherQuery = _context.Teachers
                .Where(t => t.TeacherSubjects.Any(ts =>
                    ts.Subject.StudentSubjects.Any(ss => topStudents.Contains(ss.StudentId))));

            return await teacherQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<TeacherDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }