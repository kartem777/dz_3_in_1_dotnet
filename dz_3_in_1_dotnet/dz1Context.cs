using Microsoft.EntityFrameworkCore;

namespace dz_3_in_1_dotnet
{
    public class dz1Context : DbContext
    {
        public DbSet<FileData> Files { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb;Database = dz1dot;Trusted_Connection=true;");
        }
    }

}
