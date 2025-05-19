using CoinPlanner.DataBase.ModelsDb;
using CoinPlanner.DataBase.ModelsDB;
using Microsoft.EntityFrameworkCore;

namespace CoinPlanner.DataBase;

public class AppDbContext : DbContext
{
    public DbSet<Operations> operations { get; set; } = null!;
    public DbSet<Plans> plans { get; set; } = null!;
    public DbSet<Categories> categories { get; set; } = null!;
    public DbSet<Fixations> fixations { get; set; } = null!;

    public AppDbContext()
        => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=coinplannerdb;Username=postgres;Password=Gm04stb");
}
