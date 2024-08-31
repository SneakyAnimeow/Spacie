using App.Data.Sqlite.Context;
using App.Data.Sqlite.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Data.Sqlite.Repositories.CustomValueRepository;

public class CustomValueRepository : RepositoryBase<CustomValueRepository>, ICustomValueRepository
{
    private readonly ISqliteDbContext _context;
    
    public CustomValueRepository(ISqliteDbContext context, ILogger<CustomValueRepository> logger) : base(context, logger)
    {
        _context = context;
    }
    
    public async Task<CustomValue> AddAsync(CustomValue entity)
    {
        _context.CustomValues.Add(entity);
        await SaveChangesAsync();
        
        return entity;
    }

    public async Task<CustomValue> UpdateAsync(CustomValue entity)
    {
        _context.CustomValues.Update(entity);
        await SaveChangesAsync();
        
        return entity;
    }

    public async Task<bool> DeleteAsync(CustomValue entity)
    {
        _context.CustomValues.Remove(entity);
        await SaveChangesAsync();
        
        return true;
    }

    public async ValueTask<CustomValue?> GetByIdAsync(int id)
    {
        return await _context.CustomValues.FindAsync(id);
    }

    public async Task<List<CustomValue>> GetAllAsync()
    {
        return await _context.CustomValues.ToListAsync();
    }
}