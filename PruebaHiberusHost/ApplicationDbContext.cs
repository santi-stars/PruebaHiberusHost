using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PruebaHiberusHost.Entities;

namespace PruebaHiberusHost
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sum>().HasKey(s => s.SKU); // Establecer SKU de Sum como clave primaria

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Sum) // Establecer la relación
                  .WithMany(s => s.Transactions) // Indicar propiedad de navegación en Sum (si la tienes)
                .HasForeignKey(t => t.SKU); // Clave foránea que referencia SKU de Sum

            // Configurar una clave primaria compuesta en ExchangeRate
            modelBuilder.Entity<ExchangeRate>()
                    .HasKey(e => new
                    {
                        e.FromCurrency,
                        e.ToCurrency
                    });
        }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Sum> Sums { get; set; }
    }
}
