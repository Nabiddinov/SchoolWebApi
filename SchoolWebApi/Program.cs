using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SchoolWebApi.Infrastructure.Data;
using SchoolWebApi.Application.Services.StudentService;
using SchoolWebApi.Application.Services.TeacherService;
using SchoolWebApi.Application.Mapping;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var conn = configuration.GetConnectionString("DefaultConnection");


builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(conn));


builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "School API", Version = "v1" });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    SeedData.Seed(db);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();