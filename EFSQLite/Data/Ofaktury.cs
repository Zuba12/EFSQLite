using EFSQLite.Models;
using Microsoft.EntityFrameworkCore;

namespace EFSQLite.Data
{
    public class Ofaktury : DbContext
    {
        public Ofaktury()
        {
            Database.EnsureCreated();// Zajistí vytvoření tabulky
        }

        public DbSet<Ofaktury2> Students { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MY3.db");
            optionsBuilder.UseSqlite($"Data Source = {path}");
        }
    }
}
