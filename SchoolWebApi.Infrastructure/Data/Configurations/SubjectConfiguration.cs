using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolWebApi.Domain.Entities;

namespace SchoolWebApi.Infrastructure.Data.Configurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable("Subjects");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.GradeLevel)
            .IsRequired();

        builder.HasMany(s => s.TeacherSubjects)
            .WithOne(ts => ts.Subject)
            .HasForeignKey(ts => ts.SubjectId);

        builder.HasMany(s => s.StudentSubjects)
            .WithOne(ss => ss.Subject)
            .HasForeignKey(ss => ss.SubjectId);
    }
}