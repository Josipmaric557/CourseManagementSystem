using System;
using System.Collections.Generic;
using CourseManagementSystem.Enums;

namespace CourseManagementSystem.Models.entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public DateTime? LastLoginAt { get; set; }

        //M:N
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        //M:N
        public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
        //1:N
        public ICollection<Review> Reviewes { get; set; } = new List<Review>();
        
    }
}
