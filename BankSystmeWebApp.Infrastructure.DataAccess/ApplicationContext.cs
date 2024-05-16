using Microsoft.EntityFrameworkCore;
using Models.Accounts;
using Npgsql;

namespace DataAccess;

public class ApplicationContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql("Host=localhost;Port=6432;Database=postgres;Username=postgres;Password=postgres");
    }
}