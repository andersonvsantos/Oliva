using Microsoft.EntityFrameworkCore;
using Oliva.Data;
using Oliva.Models;
using Oliva.Models.Dtos.Auth;
using Oliva.Service;

namespace Oliva.Service
{
    public class AuthService
    {
        private readonly AppDbContext _databaseContext;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;

        public AuthService(AppDbContext databaseContext, IConfiguration configuration, TokenService tokenService)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<object> AuthenticateAsync(LoginDto loginDto)
        {
            var user = await _databaseContext.Users
                .FirstOrDefaultAsync(udto => udto.Email == loginDto.Email);

            if (user == null)
            {
                throw new Exception("Invalid email or password.");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password + _configuration["Security:PasswordPepper"], user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new Exception("Invalid email or password.");
            }

            var token = _tokenService.Generate(user);

            return new { Token = token, User = new { user.Name, user.Email, user.Role } };
        }

        public async Task ResetPasswordAsync(ResetPasswordDto resetDto)
        {
            var userDb = await _databaseContext.Users
                .FirstOrDefaultAsync(u => u.Email == resetDto.Email);

            if (userDb == null || !BCrypt.Net.BCrypt.Verify(resetDto.OldPassword + _configuration["Security:PasswordPepper"], userDb.PasswordHash))
            {
                throw new Exception("Invalid email or password.");
            }

            string newHash = BCrypt.Net.BCrypt.HashPassword(resetDto.NewPassword + _configuration["Security:PasswordPepper"]);

            userDb.PasswordHash = newHash;

            await _databaseContext.SaveChangesAsync();
        }
    }
}