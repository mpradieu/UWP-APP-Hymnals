using Hymnals.Models;
using Microsoft.EntityFrameworkCore;
using Windows.Storage;

namespace Hymnals.DataLayer
{
    public class HymnalsContext : DbContext
    {
        public DbSet<Shelf> Shelves { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = "Data Source=" + ApplicationData.Current.LocalFolder.Path + "\\" + App.DefaultDBName;

            optionsBuilder.UseSqlite(connStr);
        }
    }
}
