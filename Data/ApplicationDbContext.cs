using Microsoft.EntityFrameworkCore;
using Zone.Models;

namespace Zone.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet <Store> Stores { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<IteminStore>IteminStores { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<IteminStore>(x =>
             x.HasKey(x=> new { x.StoreId, x.ItemId }));
        }

    }
}
