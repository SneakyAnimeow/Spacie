using App.Data.Sqlite.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace App.Data.Sqlite.Context;

/// <summary>
/// Represents the interface for the SQLite database context.
/// </summary>
public interface ISqliteDbContext : ICurrentDbContext
{
    DbSet<CustomValue> CustomValues { get; set; }
    
    DbSet<TTT> TTTs { get; set; }
}