using Microsoft.EntityFrameworkCore;

namespace dz_3_in_1_dotnet
{
    public class dz2Context : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb;Database = dz2dot;Trusted_Connection=true;");
        }
    }

}
