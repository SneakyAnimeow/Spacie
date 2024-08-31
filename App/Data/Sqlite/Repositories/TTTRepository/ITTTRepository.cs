using App.Data.Sqlite.Entities;

namespace App.Data.Sqlite.Repositories.TTTRepository;

public interface ITTTRepository : IRepository<TTT>
{
    ValueTask<TTT?> GetByIdAsync(Guid id);
    
    Task<TTT?> GetByGuildIdAndChannelIdAndUserIdAsync(ulong guildId, ulong channelId, ulong userId);
}