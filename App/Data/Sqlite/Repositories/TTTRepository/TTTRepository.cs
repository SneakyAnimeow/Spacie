using App.Data.Sqlite.Context;
using App.Data.Sqlite.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Data.Sqlite.Repositories.TTTRepository;

public class TTTRepository(ISqliteDbContext context, ILogger<TTTRepository> logger) : RepositoryBase<TTTRepository>(context, logger), ITTTRepository
{
    private readonly ISqliteDbContext _context = context;

    public async Task<TTT> AddAsync(TTT entity)
    {
        _context.TTTs.Add(entity);
        await SaveChangesAsync();
        
        return entity;
    }

    public async Task<TTT> UpdateAsync(TTT entity)
    {
        _context.TTTs.Update(entity);
        await SaveChangesAsync();
        
        return entity;
    }

    public async Task<bool> DeleteAsync(TTT entity)
    {
        _context.TTTs.Remove(entity);
        await SaveChangesAsync();
        
        return true;
    }

    [Obsolete("This method is not implemented.", true)]
    public ValueTask<TTT?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TTT>> GetAllAsync()
    {
        return await _context.TTTs.ToListAsync();
    }

    public async ValueTask<TTT?> GetByIdAsync(Guid id)
    {
        return await _context.TTTs.FindAsync(id);
    }

    public async Task<TTT?> GetByGuildIdAndChannelIdAndUserIdAsync(ulong guildId, ulong channelId, ulong userId)
    {
        return await _context.TTTs.FirstOrDefaultAsync(x => x.GuildId == guildId && x.ChannelId == channelId && x.UserId == userId);
    }
}