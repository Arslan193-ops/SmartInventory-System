using Microsoft.EntityFrameworkCore;
using SmartInventory_System.Models;

namespace SmartInventory_System.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<StockMovement> StockMovements => Set<StockMovement>();
        public DbSet<LowStockAlert> LowStockAlerts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique index on SKU (normalized)
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Sku)
                .IsUnique();

            // Configure relationships
            modelBuilder.Entity<StockMovement>()
                .HasOne(sm => sm.Product)
                .WithMany()
                .HasForeignKey(sm => sm.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: set default UTC for CreatedAt if using SQL Server default
            // modelBuilder.Entity<StockMovement>()
            //     .Property(sm => sm.CreatedAt)
            //     .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
