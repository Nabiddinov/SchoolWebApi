using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolWebApi.Infrastructure.Data;
using SchoolWebApi.Application.Dtos;
using SchoolWebApi.Domain.Entities;

namespace SchoolWebApi.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public CitiesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetCities()
        {
            var cities = await _context.Cities.Where(c => !c.IsDeleted).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<CityDto>>(cities));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CityDto>> GetCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null || city.IsDeleted) return NotFound();
            return Ok(_mapper.Map<CityDto>(city));
        }


        [HttpPost]
        public async Task<ActionResult<CityDto>> CreateCity([FromBody] CityDto dto)
        {
            var city = new City { Name = dto.Name, CreatedDate = DateTime.UtcNow };
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCity), new { id = city.Id }, _mapper.Map<CityDto>(city));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCity(int id, [FromBody] CityDto dto)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null || city.IsDeleted) return NotFound();
            city.Name = dto.Name;
            city.LastUpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null || city.IsDeleted) return NotFound();
            city.IsDeleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }