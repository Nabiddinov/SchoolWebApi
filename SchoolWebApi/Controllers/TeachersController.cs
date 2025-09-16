using Microsoft.AspNetCore.Mvc;
using SchoolWebApi.Application.Dtos.TeacherDTOs;
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
    public async Task<ActionResult<TeacherDto>> GetById(int id)
    {
        var teacher = await _teacherService.GetByIdAsync(id);
        return teacher == null ? NotFound(new { error = $"Teacher {id} not found" }) : Ok(teacher);
    }

    [HttpPost]
    public async Task<ActionResult<TeacherDto>> Create(TeacherCreateDto dto)
    {
        var created = await _teacherService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TeacherDto>> Update(int id, TeacherDto dto)
    {
        var updated = await _teacherService.UpdateAsync(id, dto);
        return updated == null ? NotFound(new { error = $"Teacher {id} not found" }) : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _teacherService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound(new { error = $"Teacher {id} not found" });
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<TeacherDto>>> Filter(
        [FromQuery] int? genderId, 
        [FromQuery] int? cityId,
        [FromQuery] string? subjectName)
        => Ok(await _teacherService.FilterAsync(genderId, cityId, subjectName));
    
    [HttpGet("by-top-students")]
    public async Task<ActionResult<IEnumerable<TeacherDto>>> GetTeachersByTopStudents(
        [FromQuery] bool topHighest = true,
        [FromQuery] int topStudents = 5,
        [FromQuery] int topTeachers = 10)
    {
        var result = await _teacherService.GetTeachersByTopStudentsAsync(topHighest, topStudents, topTeachers);
        return Ok(result);
    }
}
