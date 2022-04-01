using testTaskUzkikh.Models;

namespace testTaskUzkikh.DbRepository.Interfaces
{
    public interface IUserRepository
    {
        Task AddNewAsync(string email);
        Task<User> GetByIdAsync(long id);

        Task<bool> CheckIfExist(string email);
    }
}
