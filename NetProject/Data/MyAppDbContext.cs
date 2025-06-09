using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetProject.Models;

namespace NetProject.Data
{
    public class MyAppDbContext : IdentityDbContext<ApplicationUser>
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options)
            : base(options)
        {
        }

        // Tutaj definiujesz wszystkie "tabele"
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ServiceOrder> ServiceOrders { get; set; }
        public DbSet<ServiceTask> ServiceTasks { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<ServiceTaskPart> ServiceTaskParts { get; set; }
        public DbSet<UsedPart> UsedParts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }


        // Opcjonalnie konfiguracje np. relacji kluczowych
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WorkOrder>()
                .HasOne(w => w.Vehicle)
                .WithMany()
                .HasForeignKey(w => w.VehicleId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ to dodaj

            builder.Entity<WorkOrder>()
                .HasOne(w => w.Customer)
                .WithMany()
                .HasForeignKey(w => w.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ to dodaj

            builder.Entity<WorkOrder>()
                .HasOne(w => w.AssignedMechanic)
                .WithMany()
                .HasForeignKey(w => w.AssignedMechanicId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ to dodaj
            builder.Entity<ServiceTask>()
                .HasOne(st => st.WorkOrder)
                .WithMany(wo => wo.ServiceTasks)
                .HasForeignKey(st => st.WorkOrderId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<UsedPart>()
                .HasOne(up => up.Part)
                .WithMany()
                .HasForeignKey(up => up.PartId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UsedPart>()
                .HasOne(up => up.ServiceTask)
                .WithMany(t => t.UsedParts)
                .HasForeignKey(up => up.ServiceTaskId);
            
        }
    }
}