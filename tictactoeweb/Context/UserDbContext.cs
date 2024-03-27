using Microsoft.EntityFrameworkCore;
using tictactoeweb.Models;

namespace tictactoeweb.Context
{
    public class UserDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
