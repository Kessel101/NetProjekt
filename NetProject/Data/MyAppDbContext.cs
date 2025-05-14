using Microsoft.EntityFrameworkCore;
//using NetProject.Models;

namespace NetProject.Data
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options)
            : base(options)
        {
        }

        // DbSety: każda właściwość to tabelka w bazie
        //public DbSet<Product> Products { get; set; }
        //public DbSet<Order> Orders { get; set; }
        // … dodaj kolejne DbSet<T> dla innych modeli
    }
}