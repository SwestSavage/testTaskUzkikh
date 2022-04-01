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

        public async Task AddNewAsync(string email)
        {
            if (email is null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                if (!await context.Users.AnyAsync(u => u.Email == email))
                {
                    await context.Users.AddAsync(new User() { Email = email });
                }
                else
                {
                    throw new ArgumentException($"User with suggested email already exist");
                }

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

        public async Task<bool> CheckIfExist(string email)
        {
            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                if (await context.Users.AnyAsync(u => u.Email == email))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
