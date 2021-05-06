using System;
using System.Collections.Generic;
using System.Text;

namespace TestApplication.Service.Student
{
    public class StudentService: IStudentService
    {
        private readonly ApplicationDbContext _context;
        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }
    }
    public interface IStudentService
    {

    }
}
