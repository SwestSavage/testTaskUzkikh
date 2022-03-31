using testTaskUzkikh.Models;

namespace testTaskUzkikh.DbRepository.Interfaces
{
    public interface IUnpRepository
    {
        Task AddNewAsync(UNP unp);
        Task UpdateAsync(long vunp, DateTime dlikv, string vlikv, string vkods);
        Task UpdateUserIdAsync(long vunp, string userEmal);

        Task<List<UNP>> GetAllWithAssignedUsersAsync();

        Task<bool> CheckIfExistAsync(long vunp);
        Task<UNP> GetAsync(long vunp);
    }
}
