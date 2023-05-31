using PostSearchPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace PostSearchPlatform.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Post> Post { get; set; } = default!;
    public DbSet<Click> Click { get; set; } = default!;
}
