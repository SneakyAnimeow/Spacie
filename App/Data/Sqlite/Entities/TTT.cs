namespace App.Data.Sqlite.Entities;

public class TTT
{
    public Guid Id { get; set; }
    
    public ulong GuildId { get; set; }
    
    public ulong ChannelId { get; set; }
    
    public ulong UserId { get; set; }
    
    /// <summary>
    /// Represents the board of the Tic-Tac-Toe game.
    /// </summary>
    /// <example>
    /// [0, 0, 1, 0, 2, 0, 0, 0, 0] represents the following board:
    /// <code>
    /// 0 | 0 | O
    /// 0 | X | 0
    /// 0 | 0 | 0
    /// </code>
    /// </example>
    /// <remarks>
    /// 0 represents an empty cell, 1 represents a cell with an "O" and 2 represents a cell with an "X".
    /// </remarks>
    public List<int> Board { get; set; } = [];
}