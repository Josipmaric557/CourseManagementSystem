using System;
using System.Threading;
using System.Threading.Tasks;
using CourseManagementSystem.Enums;
using CourseManagementSystem.Models.entities;
using Microsoft.EntityFrameworkCore;
namespace CourseManagementSystem.DbContextModel
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Test> Tests => Set<Test>();
        public DbSet<TestQuestion> TestQuestions => Set<TestQuestion>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserCourse> UserCourses => Set<UserCourse>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            

            modelBuilder.Entity<Course>()
                .HasOne(co => co.Category)
                .WithMany(cat => cat.Courses)
                .HasForeignKey(co => co.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Course)
                .WithMany(co => co.Lessons)
                .HasForeignKey(l => l.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(re => re.User)
                .WithMany(usr => usr.Reviewes)
                .HasForeignKey(re => re.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(re => re.Course)
                .WithMany(co => co.Reviews)
                .HasForeignKey(re => re.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Test>()
                .HasOne(te => te.Lesson)
                .WithMany(le => le.Tests)
                .HasForeignKey(te => te.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TestQuestion>()
                .HasOne(teq => teq.Test)
                .WithMany(q => q.Questions)
                .HasForeignKey(teq => teq.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserCourse>()
                .HasOne(usc => usc.User)
                .WithMany(u => u.UserCourses)
                .HasForeignKey(usc => usc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserCourse>()
                .HasOne(usc => usc.Course)
                .WithMany(co => co.UserCourses)
                .HasForeignKey(usc => usc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(usr => usr.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(usr => usr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(usr => usr.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(usr => usr.RoleId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<UserCourse>()
                .HasIndex(uc => new { uc.UserId, uc.CourseId })
                .IsUnique();

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<User>()
                .HasIndex(us => us.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(us => us.Username)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(cat => cat.Name)
                .IsUnique();

            modelBuilder.Entity<Review>()
                .HasIndex(re => new { re.UserId, re.CourseId })
                .IsUnique();

            modelBuilder.Entity<Course>()
                .Property(co => co.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<UserCourse>()
                .Property(usc => usc.Status)
                .HasConversion<string>();




            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = RoleE.Admin },
                new Role { Id = 2, Name = RoleE.User }
                );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Introduction to C# basics", Description = "I suggest this course to all students who are learning basics" },
                new Category { Id = 2, Name = "C# .Net", Description = "For all who want to know more about web and c#" }
                );
        }

        public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Modified)
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            return base.SaveChangesAsync(ct);
        }
    }

}
