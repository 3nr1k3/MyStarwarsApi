using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyStarwarsApi.Models;
using MyStarwarsApi.Repo;

namespace MyStarwarsApi.Context{
    public class SqliteDbContext : IdentityDbContext<ApplicationUser> {

        private static bool _databaseChecked;

        public SqliteDbContext(DbContextOptions options) 
        : base(options){}

        public DbSet<Character> Characters { get;set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public void EnsureDatabaseCreated(){
            if(!_databaseChecked){
                _databaseChecked = true;
                this.Database.EnsureCreated();
                CharacterRepository.FillCharacterRepository(this);
            }
        }
    }
}