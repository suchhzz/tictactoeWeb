using Microsoft.EntityFrameworkCore;
using tictactoeweb.Context;
using tictactoeweb.Models.AuthorizationModels;
using tictactoeweb.Models.MainModels;

namespace tictactoeweb.Services
{
    public class UserServices
    {
        private UserDbContext _context;

        public UserServices(UserDbContext context)
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
    }
}
