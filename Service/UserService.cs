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

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _databaseContext.Users.ToListAsync();
        }

        public async Task<User?> GetUserByUUIDAsync(string UUID)
        {
            return await _databaseContext.Users.FirstOrDefaultAsync(user => user.UUID == UUID);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _databaseContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User> CreateNewUserAsync(CreateUserDto userDto)
        {
            var userDb = await GetUserByEmailAsync(userDto.Email);
            
            if (userDb != null) 
            {
                throw new Exception("There is already a created user with this email.");
            }

            var newUser = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password + _configuration["Security:PasswordPepper"]),
                Role = "User",
                UUID =  Guid.NewGuid().ToString()
            };

            _databaseContext.Users.Add(newUser);
            await _databaseContext.SaveChangesAsync();
            
            return newUser;
        }

        public async Task UpdateUserAsync(string UUID, UpdateUserDto updateData)
        {
            var userDb = await _databaseContext.Users.FirstOrDefaultAsync(user => user.UUID == UUID);
            
            if (userDb == null) 
            {
                throw new Exception("User not found for update.");
            }

            _databaseContext.Entry(userDb).CurrentValues.SetValues(updateData);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string UUID)
        {
            var userDb = await _databaseContext.Users.FirstOrDefaultAsync(user => user.UUID == UUID);
            
            if (userDb == null) 
            {
                throw new Exception("User not found for deleting.");
            }

            _databaseContext.Users.Remove(userDb);
            await _databaseContext.SaveChangesAsync();
        }
    }
}