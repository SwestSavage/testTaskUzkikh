using Microsoft.EntityFrameworkCore;
using testTaskUzkikh.DbRepository.Interfaces;
using testTaskUzkikh.Models;

namespace testTaskUzkikh.DbRepository.Implementations
{
    public class UnpRepository : BaseRepository, IUnpRepository
    {
        public UnpRepository(string connectionString, IRepositoryContextFactory repositoryContextFactory) : base(connectionString, repositoryContextFactory)
        {
        }

        public async Task AddNewAsync(UNP unp)
        {
            if (unp is null)
            {
                throw new ArgumentNullException(nameof(unp));
            }

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                await context.Unps.AddAsync(unp);

                await context.SaveChangesAsync();
            }    
        }

        public async Task<bool> CheckIfExistAsync(long vunp)
        {
            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                if (await context.Unps.AnyAsync(u => u.VUNP == vunp))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<UNP> GetAsync(long vunp)
        {
            UNP? unp;

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                unp = await context.Unps.Include(u => u.User).FirstOrDefaultAsync(u => u.VUNP == vunp);
            }

            if (unp is null)
            {
                throw new NullReferenceException("Cannot find unp with suggested vunp");
            }

            return unp;
        }

        public async Task UpdateAsync(long vunp, DateTime dlikv, string vlikv, string vkods)
        {
            UNP? unp;

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                unp = await context.Unps.Include(u => u.User).FirstOrDefaultAsync(u => u.VUNP == vunp);

                if (unp is null)
                {
                    throw new NullReferenceException("Cannot find unp with suggested vunp");
                }

                unp.VKODS = vkods;
                unp.DLIKV = dlikv;
                unp.VLIKV = vlikv;

                await context.SaveChangesAsync();
            }
        }
    }
}
