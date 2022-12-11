using AuthTestAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthTestAPI.Data
{
    public class UserDBContext : DbContext
    {
        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        { }
        public DbSet<User> User { get; set; }
    }
}
