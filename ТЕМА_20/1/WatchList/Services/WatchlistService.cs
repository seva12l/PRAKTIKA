using WatchList.Models;

namespace WatchList.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly List<WatchItem> items = new()
        {
            new WatchItem { Id = 1, Title = "Интерстеллар", Genre = "Фантастика", Type = WatchType.Movie, Status = WatchStatus.Watched, DateAdded = DateTime.Now.AddDays(-30) },
            new WatchItem { Id = 2, Title = "Во все тяжкие", Genre = "Драма", Type = WatchType.Series, Status = WatchStatus.Watching, DateAdded = DateTime.Now.AddDays(-10) },
            new WatchItem { Id = 3, Title = "Дюна: часть вторая", Genre = "Фантастика", Type = WatchType.Movie, Status = WatchStatus.Planned, DateAdded = DateTime.Now.AddDays(-3) },
            new WatchItem { Id = 4, Title = "Очень странные дела", Genre = "Триллер", Type = WatchType.Series, Status = WatchStatus.Planned, DateAdded = DateTime.Now.AddDays(-1) }
        };

        public IReadOnlyList<WatchItem> GetAll(WatchStatus? status = null)
        {
            return status.HasValue
                ? items.Where(i => i.Status == status.Value).ToList()
                : items.ToList();
        }

        public WatchItem? GetById(int id) => items.FirstOrDefault(i => i.Id == id);

        public WatchItem Add(WatchItem item)
        {
            item.Id = items.Count == 0 ? 1 : items.Max(i => i.Id) + 1;
            item.DateAdded = DateTime.Now;
            items.Add(item);
            return item;
        }

        public bool Remove(int id)
        {
            var item = GetById(id);
            if (item == null) return false;
            items.Remove(item);
            return true;
        }

        public bool MarkAsWatched(int id)
        {
            var item = GetById(id);
            if (item == null) return false;
            item.Status = WatchStatus.Watched;
            return true;
        }
    }
}
