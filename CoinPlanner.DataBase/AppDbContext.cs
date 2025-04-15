using CoinPlanner.DataBase.ModelsDB;
using Microsoft.EntityFrameworkCore;

namespace CoinPlanner.DataBase;

public class AppDbContext : DbContext
{
    public DbSet<Operations> Operations { get; set; } = null!;
    public DbSet<Plans> Plans { get; set; } = null!;

    public AppDbContext()
        => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=coinplannerdb;Username=postgres;Password=Gm04stb");
}
