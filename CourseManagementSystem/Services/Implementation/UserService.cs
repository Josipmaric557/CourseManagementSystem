using System;
using System.Collections.Generic;
using CourseManagementSystem.DTOs.Auth;
using CourseManagementSystem.DTOs.UserD;
using CourseManagementSystem.Enums;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Implementation;
using CourseManagementSystem.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using CourseManagementSystem.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CourseManagementSystem.Services.Implementation
{
    public class UserService : IUserService
    {

        private readonly IUserRepository userRepository;
        private readonly ILogger<UserService> logger;
        

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IConfiguration configuration,
            ILogger<UserService> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllAsync()
        {
            var users = await userRepository.GetAllAsync();
            return users.Select(toResponseDto);
        }

        public async Task<UserResponseDTO?> GetByEmailAsync(string email)
        {
            var user = await userRepository.GetByEmailAsync(email)
                       ?? throw new KeyNotFoundException($"Korisnik s emailom '{email}' nije pronađen.");
            return toResponseDto(user);
        }

        public async Task<UserResponseDTO?> GetByIdAsync(int id)
        {
            var user = await userRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Korisnik s ID-om {id} nije pronađen.");

            return toResponseDto(user);
        }

        public async Task<UserResponseDTO?> GetByUsernameAsync(string username)
        {
            var user = await userRepository.GetByUsernameAsync(username)
                ?? throw new KeyNotFoundException("User s tim username-om ne postoji.");

            return toResponseDto(user);
        }

        public async Task<UserResponseDTO> UpdateAsync(int id, UserUpdateDTO dto)
        {
            var user = await userRepository.GetByIdAsync(id)
           ?? throw new KeyNotFoundException($"Korisnik s ID-om {id} nije pronađen.");
            
            if (dto.Username is not null)
            {
                if (string.IsNullOrWhiteSpace(dto.Username))
                    throw new ArgumentException(
                        "Username ne može biti prazan.",
                        nameof(dto.Username));

                if (dto.Username.Length < 3)
                    throw new ArgumentException(
                        "Username mora imati najmanje 3 znaka.",
                        nameof(dto.Username));

                user.Username = dto.Username;
            }


            if (dto.Email is not null)
            {
                if (string.IsNullOrWhiteSpace(dto.Email))
                    throw new ArgumentException(
                        "Email ne može biti prazan.",
                        nameof(dto.Email));

                if (!System.Text.RegularExpressions.Regex.IsMatch(
                        dto.Email,
                        @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    throw new ArgumentException(
                        "Email nije ispravnog formata.",
                        nameof(dto.Email));
                }

                user.Email = dto.Email;
            }


            if (dto == null)
                throw new ArgumentNullException(nameof(dto));


            if (dto.Username is not null)
                user.Username = dto.Username;

            if (dto.Email is not null)
                user.Email = dto.Email;

            if (dto.Password is not null)
                user.PasswordHash = dto.Password;


            var updated = await userRepository.UpdateAsync(user);

            logger.LogInformation("Ažuriran korisnik: ID {Id} | Username: {Username}",
                updated.Id, updated.Username);

            
            return toResponseDto(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await userRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Korisnik s ID-om {id} nije pronađen.");

            logger.LogInformation("Obrisan korisnik: ID {Id} | Username: {Username}",
                user.Id, user.Username);

            
            return await userRepository.DeleteAsync(id);
            
        }

        private static UserResponseDTO toResponseDto(User user) => new()
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            UserCoursesCount = user.UserCourses.Count,
            UserRolesCount = user.UserRoles.Count,
            ReviewesCount = user.Reviewes.Count
        };

        
    }
}
