using EFSQLite.Models;
using Microsoft.EntityFrameworkCore;

namespace EFSQLite.Data
{
    public class Faktury2 : DbContext
    {
        public Faktury2()
        {
            Database.EnsureCreated();// Zajistí vytvoření tabulky
        }

        public DbSet<Faktury> Students { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MY2.db");
            optionsBuilder.UseSqlite($"Data Source = {path}");
        }
    }
}
