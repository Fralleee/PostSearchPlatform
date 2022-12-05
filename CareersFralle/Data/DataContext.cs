using CareersFralle.Models;
using Microsoft.EntityFrameworkCore;

namespace CareersFralle.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Post> Post { get; set; } = default!;
        public DbSet<Click> Click { get; set; } = default!;
    }
}
