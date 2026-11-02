using LtdTH1.Models;
using Microsoft.EntityFrameworkCore;

namespace LtdTH1.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) { }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Learner> Learners { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Major> Majors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Major>().ToTable("Major");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Learner>().ToTable("Learner");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
        }
    }
}
