using System;
using System.Collections.Generic;
using CourseManagementSystem.DTOs.Role;
using CourseManagementSystem.Enums;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Interfaces;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace CourseManagementSystem.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleResponseDTO>> GetAllAsync()
        {
            var roles = await roleRepository.GetAllAsync();
            return roles.Select(toResponseDTO);
        }

        public async Task<RoleResponseDTO?> GetByIdAsync(int id)
        {
            if (!await roleRepository.ExistsByIdAsync(id))
                throw new KeyNotFoundException($"Nepostoji role sa id:{id}");

            var role = await roleRepository.GetByIdAsync(id);

            return toResponseDTO(role);
        }

        public async Task<RoleResponseDTO?> GetByNameAsync(RoleE name)
        {
            var role = await roleRepository.GetByNameAsync(name)
                       ?? throw new KeyNotFoundException($"Uloga '{name}' nije pronađena.");
            return toResponseDTO(role);
        }
        private static RoleResponseDTO toResponseDTO(Role role) => new()
        {
            Id = role.Id,
            Name = role.Name,
            UserRolesCount = role.UserRoles.Count
        };
    }
}