using SchoolWebApi.Domain.Entities;

namespace SchoolWebApi.Infrastructure.Data;

public static class SeedData
    {
        public static void Seed(AppDbContext db)
        {
            if (db.Cities.Any()) return; // already seeded


            var c1 = new City { Name = "Tashkent" };
            var c2 = new City { Name = "Samarkand" };
            var d1 = new Department { Name = "Computer Science" };
            var d2 = new Department { Name = "Mathematics" };
            db.Cities.AddRange(c1, c2);
            db.Departments.AddRange(d1, d2);
            db.SaveChanges();


            // subjects
            var s1 = new Subject { Name = "Algebra", GradeLevel = 10 };
            var s2 = new Subject { Name = "Physics", GradeLevel = 11 };
            var s3 = new Subject { Name = "Programming", GradeLevel = 12 };
            db.Subjects.AddRange(s1, s2, s3);
            db.SaveChanges();


            // teachers
            var t1 = new Teacher { Name = "Ali", CityId = c1.Id, BirthDate = DateTime.UtcNow.AddYears(-35), GenderId = 1 };
            var t2 = new Teacher { Name = "Leyla", CityId = c2.Id, BirthDate = DateTime.UtcNow.AddYears(-29), GenderId = 2};
            db.Teachers.AddRange(t1, t2);
            db.SaveChanges();


            db.TeachersSubjects.AddRange(
            new TeacherSubject { TeacherId = t1.Id, SubjectId = s1.Id },
            new TeacherSubject { TeacherId = t1.Id, SubjectId = s3.Id },
            new TeacherSubject { TeacherId = t2.Id, SubjectId = s2.Id }
            );


            // students
            var students = new List<Student>
{
new Student { Name = "Student A", CityId = c1.Id, BirthDate = DateTime.UtcNow.AddYears(-16), GenderId = 1, CurrentGradeLevel = 10, DepartmentId = d1.Id },
new Student { Name = "Student B", CityId = c1.Id, BirthDate = DateTime.UtcNow.AddYears(-17), GenderId = 2, CurrentGradeLevel = 11, DepartmentId = d2.Id },
new Student { Name = "Student C", CityId = c2.Id, BirthDate = DateTime.UtcNow.AddYears(-18), GenderId = 3, CurrentGradeLevel = 12, DepartmentId = d1.Id }
};
            db.Students.AddRange(students);
            db.SaveChanges();


            // marks
            var allSubjects = db.Subjects.ToList();
            var allStudents = db.Students.ToList();
            var rand = new Random();
            foreach (var st in allStudents)
            {
                foreach (var sb in allSubjects)
                {
                    db.StudentsSubjects.Add(new StudentSubject { StudentId = st.Id, SubjectId = sb.Id, Mark = Math.Round((decimal)(rand.NextDouble() * 100), 2) });
                }
            }
            db.SaveChanges();
        }
    }