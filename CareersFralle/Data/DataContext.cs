using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace CareersFralle.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            CreateTables();
        }

        private void CreateTables()
        {
            try
            {
                var databaseCreator = (Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator);
                if (databaseCreator != null)
                {
                    databaseCreator.CreateTables();
                }
            }
            catch (SqlException exception)
            {
                //A SqlException will be thrown if tables already exist. So simply ignore it.
            }
        }

        public DbSet<CareersFralle.Models.Post> Post { get; set; } = default!;
    }
}
