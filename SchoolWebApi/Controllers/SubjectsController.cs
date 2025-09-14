using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolWebApi.Infrastructure.Data;
using SchoolWebApi.Application.Dtos;
using SchoolWebApi.Domain.Entities;

namespace SchoolWebApi.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public SubjectsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects()
        {
            var subs = await _context.Subjects.Where(s => !s.IsDeleted).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<SubjectDto>>(subs));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDto>> GetSubject(int id)
        {
            var sub = await _context.Subjects.FindAsync(id);
            if (sub == null || sub.IsDeleted) return NotFound();
            return Ok(_mapper.Map<SubjectDto>(sub));
        }


        [HttpPost]
        public async Task<ActionResult<SubjectDto>> CreateSubject([FromBody] SubjectDto dto)
        {
            var sub = new Subject { Name = dto.Name, GradeLevel = dto.GradeLevel, CreatedDate = DateTime.UtcNow };
            _context.Subjects.Add(sub);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSubject), new { id = sub.Id }, _mapper.Map<SubjectDto>(sub));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(int id, [FromBody] SubjectDto dto)
        {
            var sub = await _context.Subjects.FindAsync(id);
            if (sub == null || sub.IsDeleted) return NotFound();
            sub.Name = dto.Name;
            sub.GradeLevel = dto.GradeLevel;
            sub.LastUpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var sub = await _context.Subjects.FindAsync(id);
            if (sub == null || sub.IsDeleted) return NotFound();
            sub.IsDeleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
