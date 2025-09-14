using SchoolWebApi.Domain.Common;
namespace SchoolWebApi.Domain.Entities;

public class TeacherSubject : BaseEntity
{
    public int TeacherId { get; set; }
    public Teacher? Teacher { get; set; }
    public int SubjectId { get; set; }
    public Subject? Subject { get; set; }
}