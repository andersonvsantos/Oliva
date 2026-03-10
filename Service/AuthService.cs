using Microsoft.EntityFrameworkCore;
using Oliva.Data;
using Oliva.Models;
using Oliva.Models.Dtos.Auth;

namespace Oliva.Service
{
    public class AuthService
    {
        private readonly AppDbContext _databaseContext;

        public AuthService(AppDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<object> AuthenticateAsync(LoginDto loginDto)
        {
            var user = await _databaseContext.Users
                .FirstOrDefaultAsync(udto => udto.Email == loginDto.Email);

            if (user == null)
            {
                throw new Exception("Invalid email or password.");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new Exception("Invalid email or password.");
            }

            //Implementar JWT

            return user;
        }
    }
}