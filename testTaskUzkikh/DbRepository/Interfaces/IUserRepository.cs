using testTaskUzkikh.Models;

namespace testTaskUzkikh.DbRepository.Interfaces
{
    public interface IUserRepository
    {
        Task AddNewAsync(User user);
        Task<User> GetByIdAsync(long id);
    }
}
