using SchoolWebApi.Domain.Common;

namespace SchoolWebApi.Domain.Entities;

public class StudentSubject : BaseEntity
{
    public int StudentId { get; set; }
    public Student? Student { get; set; }
    public int SubjectId { get; set; }
    public Subject? Subject { get; set; }
    public decimal Mark { get; set; }
}