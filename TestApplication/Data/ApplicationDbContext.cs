using TestApplication.Domain.StudentModels;
using Microsoft.EntityFrameworkCore;

namespace TestApplication
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Student> Student { get; set; }
        public DbSet<Subject> Subjects { get; set; }
    }
}
