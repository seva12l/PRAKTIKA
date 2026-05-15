using WatchList.Models;

namespace WatchList.Services
{
    public interface IWatchlistService
    {
        IReadOnlyList<WatchItem> GetAll(WatchStatus? status = null);
        WatchItem? GetById(int id);
        WatchItem Add(WatchItem item);
        bool Remove(int id);
        bool MarkAsWatched(int id);
    }
}
