#nullable disable
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public class ContosoUniversityContext : DbContext
    {
        public ContosoUniversityContext (DbContextOptions<ContosoUniversityContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Instructor> Instructors { get; set; }

        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Course>().ToTable(nameof(Course))
                .HasMany(c => c.Instructors)
                .WithMany(i => i.Courses);
            builder.Entity<Student>().ToTable(nameof(Student));
            builder.Entity<Instructor>().ToTable(nameof(Instructor));
        }
    }
}