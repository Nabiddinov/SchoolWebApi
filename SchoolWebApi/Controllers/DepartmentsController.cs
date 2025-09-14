using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolWebApi.Infrastructure.Data;
using SchoolWebApi.Application.Dtos;
using SchoolWebApi.Domain.Entities;

namespace SchoolWebApi.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public DepartmentsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        {
            var depts = await _context.Departments.Where(d => !d.IsDeleted).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<DepartmentDto>>(depts));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null || dept.IsDeleted) return NotFound();
            return Ok(_mapper.Map<DepartmentDto>(dept));
        }


        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment([FromBody] DepartmentDto dto)
        {
            var dept = new Department { Name = dto.Name, CreatedDate = DateTime.UtcNow };
            _context.Departments.Add(dept);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDepartment), new { id = dept.Id }, _mapper.Map<DepartmentDto>(dept));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentDto dto)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null || dept.IsDeleted) return NotFound();
            dept.Name = dto.Name;
            dept.LastUpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null || dept.IsDeleted) return NotFound();
            dept.IsDeleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }