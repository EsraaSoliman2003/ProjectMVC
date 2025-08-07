using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
namespace WebApplication3.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=StudentDepartmentDb;Trusted_Connection=True;Encrypt=False;");
        }
        
        public DbSet<Department> Departments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department Seed (10 Departments)
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Computer Science" },
                new Department { Id = 2, Name = "Business" },
                new Department { Id = 3, Name = "Engineering" },
                new Department { Id = 4, Name = "Mathematics" },
                new Department { Id = 5, Name = "Physics" },
                new Department { Id = 6, Name = "Chemistry" },
                new Department { Id = 7, Name = "Biology" },
                new Department { Id = 8, Name = "Psychology" },
                new Department { Id = 9, Name = "Economics" },
                new Department { Id = 10, Name = "History" }
            );

            // Student Seed (10 Students)
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, FullName = "Ahmed Ali", Age = 20, DepartmentId = 1, Email = "ahmed.ali@example.com" },
                new Student { Id = 2, FullName = "Sara Mohamed", Age = 22, DepartmentId = 2, Email = "sara.mohamed@example.com" },
                new Student { Id = 3, FullName = "Youssef Hany", Age = 21, DepartmentId = 1, Email = "youssef.hany@example.com" },
                new Student { Id = 4, FullName = "Laila Samir", Age = 23, DepartmentId = 4, Email = "laila.samir@example.com" },
                new Student { Id = 5, FullName = "Mona Adel", Age = 20, DepartmentId = 5, Email = "mona.adel@example.com" },
                new Student { Id = 6, FullName = "Karim Tarek", Age = 24, DepartmentId = 3, Email = "karim.tarek@example.com" },
                new Student { Id = 7, FullName = "Nour Hassan", Age = 19, DepartmentId = 7, Email = "nour.hassan@example.com" },
                new Student { Id = 8, FullName = "Omar Khaled", Age = 22, DepartmentId = 6, Email = "omar.khaled@example.com" },
                new Student { Id = 9, FullName = "Farah Nabil", Age = 21, DepartmentId = 8, Email = "farah.nabil@example.com" },
                new Student { Id = 10, FullName = "Tamer Yassin", Age = 20, DepartmentId = 9, Email = "tamer.yassin@example.com" }
            );

            // Course Seed (10 Courses)
            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, CourseName = "Introduction to Programming" },
                new Course { Id = 2, CourseName = "Database Systems" },
                new Course { Id = 3, CourseName = "Data Structures" },
                new Course { Id = 4, CourseName = "Operating Systems" },
                new Course { Id = 5, CourseName = "Linear Algebra" },
                new Course { Id = 6, CourseName = "Marketing Principles" },
                new Course { Id = 7, CourseName = "Business Ethics" },
                new Course { Id = 8, CourseName = "Quantum Physics" },
                new Course { Id = 9, CourseName = "Organic Chemistry" },
                new Course { Id = 10, CourseName = "World History" }
            );


        }


    }
}
