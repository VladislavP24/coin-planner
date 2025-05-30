using CoinPlanner.DataBase.ModelsDb;
using CoinPlanner.DataBase.ModelsDB;
using Microsoft.EntityFrameworkCore;

namespace CoinPlanner.DataBase;

public class AppDbContext : DbContext
{
    public DbSet<Operations> operations { get; set; }
    public DbSet<Plans> plans { get; set; }
    public DbSet<Categories> categories { get; set; }
    public DbSet<Fixations> fixations { get; set; }
    public DbSet<Marks> marks { get; set; }

    public AppDbContext()
        => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=MyTestDb;Username=postgres;Password=Gm04stb");
}