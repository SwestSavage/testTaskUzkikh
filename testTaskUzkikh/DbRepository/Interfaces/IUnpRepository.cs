using testTaskUzkikh.Models;

namespace testTaskUzkikh.DbRepository.Interfaces
{
    public interface IUnpRepository
    {
        Task AddNewAsync(UNP unp);
        Task UpdateAsync(long vunp, DateTime dlikv, string vlikv, string vkods);

        Task<bool> CheckIfExistAsync(long vunp);
        Task<UNP> GetAsync(long vunp);
    }
}
