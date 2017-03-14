using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyStarwarsApi.Models;

namespace MyStarwarsApi.Context{
    public class SqliteDbContext : IdentityDbContext<ApplicationUser> {

        public SqliteDbContext(DbContextOptions options) 
        : base(options){}

        public DbSet<Character> Characters { get;set; }
        public DbSet<Image> Images { get;set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}