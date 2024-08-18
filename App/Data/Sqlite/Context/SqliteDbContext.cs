using App.Data.Sqlite.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.Data.Sqlite.Context;

public sealed class SqliteDbContext : DbContext, ISqliteDbContext
{
    private readonly IConfiguration _configuration;
    
    public SqliteDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        Context = this;
        
        Database.Migrate();
    }
    
    public DbContext Context { get; }
    
    public DbSet<CustomValue> CustomValues { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_configuration.GetSection("SQLITE_DB_PATH").Get<string>());
    }
}