using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolWebApi.Domain.Entities;

namespace SchoolWebApi.Infrastructure.Data.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("Teachers");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.BirthDate)
            .IsRequired();

        builder.HasOne(t => t.City)
            .WithMany()
            .HasForeignKey(t => t.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.TeacherSubjects)
            .WithOne(ts => ts.Teacher)
            .HasForeignKey(ts => ts.TeacherId);
    }
}