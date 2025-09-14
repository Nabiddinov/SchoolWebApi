using SchoolWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SchoolWebApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

    public DbSet<City> Cities => Set<City>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<TeacherSubject> TeachersSubjects => Set<TeacherSubject>();
    public DbSet<StudentSubject> StudentsSubjects => Set<StudentSubject>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<TeacherSubject>().HasIndex(ts => new { ts.TeacherId, ts.SubjectId }).IsUnique();
        modelBuilder.Entity<StudentSubject>().HasIndex(ss => new { ss.StudentId, ss.SubjectId }).IsUnique();
    }
}