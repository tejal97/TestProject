using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApplication.Data.Enums;
using TestApplication.Domain.StudentModels;

namespace TestApplication.Extentions
{
    public class DatabaseSeeder
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            var defaultEmail = "97tejal@gmail.com";
            var studentList = new List<Student>();
            var student1 = new Student()
            {
                StudentId = "555-2abc-34la23-123",
                StudentName = "Tejal Vaijoo",
                Address = "Chabahil",
                Grade = GradeEnum.First.ToString(),
                Email = defaultEmail
            };
            studentList.Add(student1);
            var student2 = new Student()
            {
                StudentId = "f123-sda21-dgf86-134g",
                StudentName = "Ram Sitaula",
                Address = "Chabahil",
                Grade = GradeEnum.First.ToString(),
                Email = defaultEmail
            };
            studentList.Add(student2);
            var student3 = new Student()
            {
                StudentId = "ads213-sdf123-12ds-sd213",
                StudentName = "Sudip Basnet",
                Address = "Chabahil",
                Grade = GradeEnum.First.ToString(),
                Email = defaultEmail
            };
            studentList.Add(student3);
            var student4 = new Student()
            {
                StudentId = "67reg-qw12-34344-9uh23",
                StudentName = "Roshan Kharel",
                Address = "Chabahil",
                Grade = GradeEnum.First.ToString(),
                Email = defaultEmail
            };
            studentList.Add(student4);
            var student5 = new Student()
            {
                StudentId = "908nr3-1sf23-j67g65-k7t6e",
                StudentName = "Prashant Pokhrael",
                Address = "Chabahil",
                Grade = GradeEnum.First.ToString(),
                Email = defaultEmail
            };
            studentList.Add(student5);
            var student6 = new Student()
            {
                StudentId = "908jm-7mr6-356-mft78",
                StudentName = "Sunil Shrestha",
                Address = "Chabahil",
                Grade = GradeEnum.First.ToString(),
                Email = defaultEmail
            };
            studentList.Add(student6);
            var marksheetList = studentList.Select(item => {
                Random random = new Random();
                var subject = new Subject()
                {
                    StudentId = item.StudentId,
                    SubjectId = Guid.NewGuid().ToString(),
                    Maths = random.Next(0, 99),
                    Science = random.Next(0, 99),
                    Social = random.Next(0, 99),
                    Nepali = random.Next(0, 99),
                    English = random.Next(0, 99),
                    Grade= item.Grade
                 };
                subject.Percentage = StaticDataFunctions.CalculatePercentage(subject);
                subject.TotalMarks = StaticDataFunctions.CalculateTotal(subject);
                return subject;
            });
            //SEED STARTS
            foreach (var item in studentList)
            {
                if (!context.Student.Where(p => p.StudentId == item.StudentId && p.Grade == item.Grade && p.StudentName == item.StudentName).Any())
                {
                    context.Student.Add(item);
                    await context.SaveChangesAsync();
                }
            }
            foreach (var item in marksheetList)
            {
                if (!context.Subjects.Where(p => p.SubjectId == item.SubjectId && p.Grade == item.Grade && p.StudentId == item.StudentId).Any())
                {
                    context.Subjects.Add(item);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
