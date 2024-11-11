using Microsoft.EntityFrameworkCore;
using MicroWaveAPI.Models;

namespace MicroWaveAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
        public DbSet<HeatingMode> HeatingModeCollection { get; set; }
        public DbSet<Microwave> MicrowaveCollection { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                modelBuilder.Entity<HeatingMode>()
                        .HasIndex(h => h.CharIndicator)
                        .IsUnique();
        }
}