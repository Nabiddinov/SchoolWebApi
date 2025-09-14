using SchoolWebApi.Domain.Common;

namespace SchoolWebApi.Domain.Entities;

public class Teacher : BaseEntity
{
    public string Name { get; set; } = null!;
    public int CityId { get; set; }
    public City? City { get; set; }
    public DateTime BirthDate { get; set; }
    public int GenderId { get; set; }


    public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
}