using App.Data.Sqlite.Entities;
using App.Data.Sqlite.Repositories.CustomValueRepository;
using Discord.WebSocket;

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