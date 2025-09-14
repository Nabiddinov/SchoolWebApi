using Microsoft.AspNetCore.Mvc;
using SchoolWebApi.Application.Dtos;
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
    public async Task<ActionResult<StudentDto>> Get(int id)
    {
        var student = await _studentService.GetByIdAsync(id);
        return student == null ? NotFound() : Ok(student);
    }

    [HttpPost]
    public async Task<ActionResult<StudentDto>> Create(StudentDto dto)
        => Ok(await _studentService.CreateAsync(dto));

    [HttpPut("{id}")]
    public async Task<ActionResult<StudentDto>> Update(int id, StudentDto dto)
    {
        var updated = await _studentService.UpdateAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _studentService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<StudentDto>>> Filter([FromQuery] int? genderId, [FromQuery] int? departmentId,
        [FromQuery] int? cityId, [FromQuery] int? gradeLevel, [FromQuery] string? zodiacSign)
        => Ok(await _studentService.FilterAsync(genderId, departmentId, cityId, gradeLevel, zodiacSign));

    [HttpGet("top-by-subject/{subjectId}")]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetTopStudents(int subjectId, [FromQuery] int top = 10)
        => Ok(await _studentService.GetTopStudentsBySubjectAsync(subjectId, top));
}