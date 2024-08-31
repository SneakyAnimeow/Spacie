using App.Data.Sqlite.Entities;
using App.Data.Sqlite.Repositories.CustomValueRepository;
using Discord;
using Discord.WebSocket;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Color = SixLabors.ImageSharp.Color;

namespace App.Discord.Commands.TestCommand;

public class TestCommand : ITestCommand
{
    private readonly ICustomValueRepository _customValueRepository;
    
    public TestCommand(ICustomValueRepository customValueRepository)
    {
        _customValueRepository = customValueRepository;
    }
    
    public async Task HandleAsync(SocketSlashCommand command)
    {
        var name = command.Data.Options.FirstOrDefault(x => x.Name == "name")?.Value as string;
        
        if (string.IsNullOrWhiteSpace(name))
        {
            await command.RespondAsync("Name is required");
            return;
        }

        if (name == "image")
        {
            await command.DeferAsync();
            
            using var image = new Image<Rgba32>(200, 200);
            
            Star star = new(x: 100.0f, y: 100.0f, prongs: 5, innerRadii: 20.0f, outerRadii:30.0f);
            
            //draw a red square with a white cross
            image.Mutate(x => x.Fill(Color.Red, star));
            
            await using var stream = new MemoryStream();
            
            await image.SaveAsPngAsync(stream);
            
            stream.Position = 0;
            
            await command.ModifyOriginalResponseAsync(x =>
            {
                x.Content = "Here is your image";
                x.Attachments = new List<FileAttachment>
                {
                    new (stream, "image.png")
                };
            });
            
            return;
        }
        
        var value = command.Data.Options.FirstOrDefault(x => x.Name == "value")?.Value as string;
        
        var test = new CustomValue
        {
            Name = name,
            Value = value
        };
        
        await _customValueRepository.AddAsync(test);
        
        await command.RespondAsync("Test added");
    }
}