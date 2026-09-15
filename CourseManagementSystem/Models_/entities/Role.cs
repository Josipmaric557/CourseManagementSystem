using System.Collections.Generic;
using CourseManagementSystem.Enums;

namespace CourseManagementSystem.Models.entities
{
    public class Role
    {
        public int Id { get; set; }
        public RoleE Name { get; set; }

        //M:N
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
