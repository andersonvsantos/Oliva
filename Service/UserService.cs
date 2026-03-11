using Microsoft.EntityFrameworkCore;
using Oliva.Data;
using Oliva.Models;

namespace Oliva.Services
{
    public class UserService
    {
        private readonly AppDbContext _databaseContext;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _databaseContext.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _databaseContext.Users.FindAsync(userId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _databaseContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User> CreateNewUserAsync(CreateUserDto userDto)
        {
            var existingUser = await GetUserByEmailAsync(userDto.Email);
            
            if (existingUser != null) 
            {
                throw new Exception("There is already a created user with this email.");
            }

            var newUser = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password + _configuration["Security:PasswordPepper"]),
                Role = "User"
            };

            _databaseContext.Users.Add(newUser);
            await _databaseContext.SaveChangesAsync();
            
            return newUser;
        }

        public async Task UpdateUserAsync(int userId, UpdateUserDto updateData)
        {
            var userDb = await _databaseContext.Users.FindAsync(userId);
            
            if (userDb == null) 
            {
                throw new Exception("User not found for update.");
            }

            _databaseContext.Entry(userDb).CurrentValues.SetValues(updateData);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var userDb = await _databaseContext.Users.FindAsync(userId);
            
            if (userDb == null) 
            {
                throw new Exception("User not found for deleting.");
            }

            _databaseContext.Users.Remove(userDb);
            await _databaseContext.SaveChangesAsync();
        }
    }
}