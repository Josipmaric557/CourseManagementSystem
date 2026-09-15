using System;
using System.Collections.Generic;
using CourseManagementSystem.DTOs.Auth;
using CourseManagementSystem.Models.entities;
using CourseManagementSystem.Repository.Interfaces;
using CourseManagementSystem.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CourseManagementSystem.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CourseManagementSystem.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthService> logger;

        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new ArgumentException(
                    "Username je obavezan.");

            if (dto.Username.Length < 3)
                throw new ArgumentException(
                    "Username mora imati najmanje 3 znaka.");
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException(
                    "Email je obavezan.");

            if (!dto.Email.Contains("@"))
                throw new ArgumentException(
                    "Email nije ispravnog formata.");

            if (await userRepository.ExistsByEmailAsync(dto.Email))
                throw new InvalidOperationException("Korisnik s tim emailom već postoji.");

            if (await userRepository.ExistsByUsernameAsync(dto.Username))
                throw new InvalidOperationException("Korisničko ime je već zauzeto.");

            var roleUser = await roleRepository.GetByNameAsync(dto.role)
                ?? throw new InvalidOperationException("Uloga 'User' nije pronađena u bazi. Provjeri seed podatke.");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsActive = true,
                UserRoles =  new List<UserRole>
                {
                    new UserRole
                    {
                        RoleId = roleUser.Id,
                    }
                }
            };

            var created = await userRepository.CreateAsync(user);
            logger.LogInformation("Novi korisnik registriran: {Email}", dto.Email);

            return await GenerateAuthResponseAsync(created);
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var user = await userRepository.GetByEmailAsync(dto.Email)
                ?? throw new UnauthorizedAccessException("Pogrešan email ili lozinka.");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("Račun je deaktiviran.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Pogrešan email ili lozinka.");

            user.LastLoginAt = DateTime.UtcNow;
            await userRepository.UpdateAsync(user);

            logger.LogInformation("Korisnik prijavljen: {Email}", dto.Email);
            return await GenerateAuthResponseAsync(user);
        }

        public async Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentException(
                    "Refresh token je obavezan.");

            var user = await userRepository.GetByRefreshTokenAsync(refreshToken)
                ?? throw new UnauthorizedAccessException("Nevažeći refresh token.");

            if (user.RefreshTokenExpiresAt is null || user.RefreshTokenExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token je istekao. Prijavite se ponovno.");

            
            logger.LogInformation("Token osvježen za korisnika: ID {UserId} | Username: {Username}",
                user.Id, user.Username);

            return await GenerateAuthResponseAsync(user);
        }

        public async Task LogoutAsync(int userId)
        {
            var user = await userRepository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException($"Korisnik s ID-om {userId} nije pronađen.");

            user.RefreshToken = null;
            user.RefreshTokenExpiresAt = null;
            await userRepository.UpdateAsync(user);

            logger.LogInformation("Korisnik odjavljen: {UserId}", userId);
        }

        //private metode

        private async Task<AuthResponseDTO> GenerateAuthResponseAsync(User user)
        {
            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
            await userRepository.UpdateAsync(user);

            var expiryMinutes = int.Parse(configuration["Jwt:ExpiryMinutes"] ?? "15");

            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
                UserId = user.Id,
                Username = user.Username,
                Roles = user.UserRoles.Select(ur => ur.Role.Name.ToString()).ToList()
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.Username),
            };

            claims.AddRange(user.UserRoles.Select(ur =>
                new Claim(ClaimTypes.Role, ur.Role.Name.ToString())));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

            var expiryMinutes = int.Parse(configuration["Jwt:ExpiryMinutes"] ?? "15");

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );              

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
