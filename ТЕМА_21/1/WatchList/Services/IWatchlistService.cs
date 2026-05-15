using WatchList.Models;

namespace WatchList.Services
{
    public interface IWatchlistService
    {
        Task<IReadOnlyList<WatchItem>> GetAllAsync(WatchStatus? status = null);
        Task<WatchItem?> GetByIdAsync(int id);
        Task<WatchItem> AddAsync(WatchItem item);
        Task<bool> UpdateAsync(WatchItem item);
        Task<bool> RemoveAsync(int id);
        Task<bool> MarkAsWatchedAsync(int id);
    }
}
