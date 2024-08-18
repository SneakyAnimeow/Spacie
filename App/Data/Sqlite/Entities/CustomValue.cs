namespace App.Data.Sqlite.Entities;

public class CustomValue
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Value { get; set; }
}