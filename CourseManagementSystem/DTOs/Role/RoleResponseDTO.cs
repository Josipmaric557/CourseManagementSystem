using CourseManagementSystem.Enums;

namespace CourseManagementSystem.DTOs.Role
{
    public class RoleResponseDTO
    {
        public int Id { get; set; }
        public RoleE Name { get; set; }
        public int UserRolesCount { get; set; }
    }
}
