using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PruebaHiberusHost.Entities;

namespace PruebaHiberusHost
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
    }
}
