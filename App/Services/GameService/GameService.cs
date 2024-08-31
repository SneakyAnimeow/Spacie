using App.Data.Sqlite.Entities;
using App.Data.Sqlite.Repositories.TTTRepository;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Color = SixLabors.ImageSharp.Color;

namespace App.Services.GameService;

public class GameService : IGameService
{
    private readonly ILogger<GameService> _logger;
    private readonly ITTTRepository _tttRepository;
    
    public GameService(ILogger<GameService> logger, ITTTRepository tttRepository)
    {
        _logger = logger;
        _tttRepository = tttRepository;
    }

    public async Task<Image<Rgba32>> GetOrCreateTTTAsync(ulong guildId, ulong channelId, ulong userId)
    {
        var ttt = await _tttRepository.GetByGuildIdAndChannelIdAndUserIdAsync(guildId, channelId, userId);

        if (ttt is null)
        {
            ttt = new TTT
            {
                GuildId = guildId,
                ChannelId = channelId,
                UserId = userId,
                Board = []
            };
            
            await _tttRepository.AddAsync(ttt);
        }
        
        var image = new Image<Rgba32>(250, 250);
        
        //make the background white
        image.Mutate(x => x.BackgroundColor(Color.White));
        
        //draw the grid of 2 vertical and 2 horizontal lines with a 10 pixel padding,
        var lines = new List<PointF[]>
        {
            new[]
            {
                new PointF(10, 10),
                new PointF(10, 240)
            },
            new[]
            {
                new PointF(90, 10),
                new PointF(90, 240)
            },
            new[]
            {
                new PointF(170, 10),
                new PointF(170, 240)
            },
            new[]
            {
                new PointF(10, 10),
                new PointF(240, 10)
            },
            new[]
            {
                new PointF(10, 90),
                new PointF(240, 90)
            },
            new[]
            {
                new PointF(10, 170),
                new PointF(240, 170)
            }
        };
        
        foreach (var line in lines)
        {
            image.Mutate(x => x.DrawLine(Color.Black, 2, line[0], line[1]));
        }
        
        return image;
    }
}