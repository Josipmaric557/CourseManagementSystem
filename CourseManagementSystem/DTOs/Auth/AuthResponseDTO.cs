using System;
using System.Collections.Generic;
using CourseManagementSystem.DTOs.UserD;
using CourseManagementSystem.Models.entities;
namespace CourseManagementSystem.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public UserResponseDTO User { get; set; } = null!;
        public List<string> Roles { get; set; } = new();
    }
}
