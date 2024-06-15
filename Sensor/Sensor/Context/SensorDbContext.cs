using Microsoft.EntityFrameworkCore;
using Sensor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
namespace Sensor.Context
{
    internal class SensorDbContext : DbContext
    {
        public DbSet<AirQualityDataDbModel> AirQualityData { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseNpgsql("Server=postgres;Port=5432;Database=IOT;Username=postgres;Password=Veljko22!!!;");

        }
        public void EnsureDatabaseCreated()
        {
            
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AirQualityDataDbModel>()
                .Property(e => e.Date)
                .HasConversion(
                    v => v.ToString("yyyy-MM-dd"),
                    v => DateOnly.ParseExact(v, "yyyy-MM-dd"));
        }
    }
}
