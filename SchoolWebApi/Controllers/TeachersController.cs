using Microsoft.AspNetCore.Mvc;
using SchoolWebApi.Application.Dtos;
using SchoolWebApi.Application.Services.TeacherService;


namespace SchoolWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeachersController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeacherDto>>> GetAll()
        => Ok(await _teacherService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<TeacherDto>> Get(int id)
    {
        var teacher = await _teacherService.GetByIdAsync(id);
        return teacher == null ? NotFound() : Ok(teacher);
    }

    [HttpPost]
    public async Task<ActionResult<TeacherDto>> Create(TeacherDto dto)
        => Ok(await _teacherService.CreateAsync(dto));

    [HttpPut("{id}")]
    public async Task<ActionResult<TeacherDto>> Update(int id, TeacherDto dto)
    {
        var updated = await _teacherService.UpdateAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _teacherService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<TeacherDto>>> Filter([FromQuery] int? genderId, [FromQuery] int? cityId,
        [FromQuery] string? subjectName)
        => Ok(await _teacherService.FilterAsync(genderId, cityId, subjectName));

    [HttpGet("top-teachers-by-top-students")]
    public async Task<ActionResult<IEnumerable<TeacherDto>>> GetTopTeachers([FromQuery] bool topHighest = true,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        => Ok(await _teacherService.GetTopTeachersByTopStudentsAsync(topHighest, page, pageSize));
}