using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using tictactoeweb.Models.MainModels;

namespace tictactoeweb.Context
{
    public class UserDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        
    }
}
