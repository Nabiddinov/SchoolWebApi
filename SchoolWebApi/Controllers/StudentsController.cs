using Microsoft.AspNetCore.Mvc;
using SchoolWebApi.Application.Dtos.StudentDTOs;
using SchoolWebApi.Application.Services.StudentService;

namespace SchoolWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetAll()
        => Ok(await _studentService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetById(int id)
    {
        var student = await _studentService.GetByIdAsync(id);
        return student == null ? NotFound(new { error = $"Student {id} not found" }) : Ok(student);
    }

    [HttpPost]
    public async Task<ActionResult<StudentDto>> Create(StudentCreateDto dto)
    {
        var created = await _studentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<StudentDto>> Update(int id, StudentUpdateDto dto)
    {
        var updated = await _studentService.UpdateAsync(id, dto);
        return updated == null ? NotFound(new { error = $"Student {id} not found" }) : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _studentService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound(new { error = $"Student {id} not found" });
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<StudentDto>>> Filter(
        [FromQuery] int? genderId, 
        [FromQuery] int? departmentId,
        [FromQuery] int? cityId, 
        [FromQuery] int? gradeLevel, 
        [FromQuery] string? zodiacSign)
    {
        var result = await _studentService.FilterAsync(genderId, departmentId, cityId, gradeLevel, zodiacSign);
        return Ok(result);
    }
    
    [HttpGet("top-by-subject/{subjectId}")]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetTopStudentsBySubject(
        int subjectId, 
        [FromQuery] bool topHighest = true,
        [FromQuery] int top = 10)
    {
        var result = await _studentService.GetTopStudentsBySubjectOrderedAsync(subjectId, topHighest, top);
        return Ok(result);
    }
}
