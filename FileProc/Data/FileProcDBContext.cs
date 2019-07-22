using Microsoft.EntityFrameworkCore;

namespace FileProc.Data
{
    internal class FileProcDbContext : DbContext
    {
        public const string ServerName = @"(localdb)\MSSQLLocalDB";
        public const string DatabaseName = "fileproc";
        public const string ConnectionString =
            @"Server=" + ServerName + ";Database=" + DatabaseName + ";Integrated Security=SSPI;";
        public const string FileProcDataTable = "dbo.FileProcData";

        public DbSet<FileProcData> FileProcData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        public void EnsureDatabaseCreated(bool dropDatabseIfModelChanges)
        {
            if (dropDatabseIfModelChanges)
                Database.EnsureDeleted();

            Database.EnsureCreated();
        }
    }
}
