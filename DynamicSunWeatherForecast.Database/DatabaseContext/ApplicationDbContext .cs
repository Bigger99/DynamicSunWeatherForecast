using DynamicSunWeatherForecast.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicSunWeatherForecast.Database.DatabaseContext;
public class ApplicationDbContext : DbContext
{
    public DbSet<Weather> Weather { get; set; }

    public ApplicationDbContext() : base()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=DynamicSunWeatherForecast;Username=postgres;Password=123");
    }
}
