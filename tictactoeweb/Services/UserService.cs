using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using tictactoeweb.Context;
using tictactoeweb.Models.AuthorizationModels;
using tictactoeweb.Models.MainModels;

namespace tictactoeweb.Services
{
    public class UserService
    {
        private UserDbContext _context;

        public UserService(UserDbContext context)
        {
            _context = context;
        }

        public Task<User> GetUserById(Guid Id)
        {
            var user = _context.Users.FirstOrDefaultAsync(x => x.Id == Id);

            return user;
        }

        public async Task<User> GetUserByUsername(string? Username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == Username);

            return user;
        }

        public async Task<Role> GetRoleByName(string roleName = "user")
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            return role;
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetRegisteredUser(LoginModel model)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);

            return user;
        }

        public async Task UpdateUser(User user)
        {
             _context.Users.Update(user);
             await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task DeleteUserById(string id)
        {
            var deleteUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(id));

            if (deleteUser != null)
            {
                _context.Users.Remove(deleteUser);
                await _context.SaveChangesAsync();
            }
        }
    }
}
