using Microsoft.EntityFrameworkCore;
using testTaskUzkikh.DbRepository.Interfaces;
using testTaskUzkikh.Models;

namespace testTaskUzkikh.DbRepository.Implementations
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(string connectionString, IRepositoryContextFactory repositoryContextFactory) : base(connectionString, repositoryContextFactory)
        {
        }

        public async Task AddNewAsync(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                await context.Users.AddAsync(user);

                await context.SaveChangesAsync();
            }
        }

        public async Task<User> GetByIdAsync(long id)
        {
            User? user;

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                user = await context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            }

            if (user is null)
            {
                throw new NullReferenceException("Cannot find user with suggested id");
            }

            return user;
        }
    }
}
