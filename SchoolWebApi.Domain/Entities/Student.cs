using SchoolWebApi.Domain.Common;

namespace SchoolWebApi.Domain.Entities;

public class Student : BaseEntity
{
    public string Name { get; set; } = null!;
    public int CityId { get; set; }
    public City? City { get; set; }
    public DateTime BirthDate { get; set; }
    public int GenderId { get; set; }
    public int CurrentGradeLevel { get; set; }
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }


    public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
}