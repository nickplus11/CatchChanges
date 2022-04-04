using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CatchChangesREST;

public sealed class SourceContext : DbContext
{
    public DbSet<TableModel> Tables { get; set; }

    public SourceContext()
    {
    }

    public SourceContext(DbContextOptions<SourceContext> options)
        : base(options)
    {
    }
}

public class TableModel
{
    [Key] [JsonPropertyName("Id")] public string Id { get; set; }
    [JsonPropertyName("Name")] public string Name { get; set; }
    [JsonPropertyName("Status")] public string Status { get; set; }

    public override string ToString() => $"Table model. ID: {Id}. Name: {Name}. Status: {Status}";
}