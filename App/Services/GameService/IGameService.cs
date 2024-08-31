using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace App.Services.GameService;

public interface IGameService
{
    Task<Image<Rgba32>> GetOrCreateTTTAsync(ulong guildId, ulong channelId, ulong userId);
}