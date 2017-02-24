
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyStarwarsApi.Models;

namespace MyStarwarsApi.Context{
    public class SqliteDbContext : DbContext {
        public DbSet<Character> Characters { get;set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "MySwApi.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            
            optionsBuilder.UseSqlite(connection);
        }
    }
}