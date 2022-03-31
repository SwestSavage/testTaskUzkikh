using Microsoft.EntityFrameworkCore;
using testTaskUzkikh.Models;

namespace testTaskUzkikh.DbRepository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UNP> Unps { get; set; }
    }
}
