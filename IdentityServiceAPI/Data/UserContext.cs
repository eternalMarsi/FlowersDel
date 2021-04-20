using IdentityServiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityServiceAPI.Data
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
    }
}
