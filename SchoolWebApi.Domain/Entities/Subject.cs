using SchoolWebApi.Domain.Common;

namespace SchoolWebApi.Domain.Entities;

public class Subject : BaseEntity
{
    public string Name { get; set; } = null!;
    public int GradeLevel { get; set; }


    public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
    public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
}