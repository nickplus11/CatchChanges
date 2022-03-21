using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CatchChangesREST;

public sealed class SourceContext : DbContext
{
    public DbSet<TableModel> Tables { get; set; }

    public SourceContext()
    {
        Database.EnsureCreated();
    }

    public SourceContext(DbContextOptions<SourceContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}

public class TableModel
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }

    public override string ToString() => $"Table model. ID: {Id}. Name: {Name}.";
}