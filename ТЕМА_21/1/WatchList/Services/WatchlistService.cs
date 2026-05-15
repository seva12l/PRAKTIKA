using Microsoft.EntityFrameworkCore;
using WatchList.Data;
using WatchList.Models;

namespace WatchList.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly AppDbContext db;

        public WatchlistService(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<IReadOnlyList<WatchItem>> GetAllAsync(WatchStatus? status = null)
        {
            var query = db.WatchItems.AsNoTracking().AsQueryable();
            if (status.HasValue)
                query = query.Where(i => i.Status == status.Value);
            return await query.OrderByDescending(i => i.DateAdded).ToListAsync();
        }

        public Task<WatchItem?> GetByIdAsync(int id)
            => db.WatchItems.FirstOrDefaultAsync(i => i.Id == id);

        public async Task<WatchItem> AddAsync(WatchItem item)
        {
            item.DateAdded = DateTime.Now;
            db.WatchItems.Add(item);
            await db.SaveChangesAsync();
            return item;
        }

        public async Task<bool> UpdateAsync(WatchItem item)
        {
            var existing = await db.WatchItems.FindAsync(item.Id);
            if (existing == null) return false;
            existing.Title = item.Title;
            existing.Genre = item.Genre;
            existing.Type = item.Type;
            existing.Status = item.Status;
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var item = await db.WatchItems.FindAsync(id);
            if (item == null) return false;
            db.WatchItems.Remove(item);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsWatchedAsync(int id)
        {
            var item = await db.WatchItems.FindAsync(id);
            if (item == null) return false;
            item.Status = WatchStatus.Watched;
            await db.SaveChangesAsync();
            return true;
        }
    }
}
