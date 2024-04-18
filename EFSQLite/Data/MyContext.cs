using EFSQLite.Models;
using Microsoft.EntityFrameworkCore;

namespace EFSQLite.Data
{
    public class MyContext : DbContext
    {
        public MyContext() 
        {
            Database.EnsureCreated();// Zajistí vytvoření tabulky
        }

        public DbSet<Student> Students { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MY.db");
            optionsBuilder.UseSqlite($"Data Source = {path}");
        }
    }
}
