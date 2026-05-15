using Microsoft.EntityFrameworkCore;
using WatchList.Models;

namespace WatchList.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<WatchItem> WatchItems { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WatchItem>().HasData(
                new WatchItem { Id = 1, Title = "Интерстеллар",        Genre = "Фантастика", Type = WatchType.Movie,  Status = WatchStatus.Watched,  DateAdded = new DateTime(2026, 4, 13) },
                new WatchItem { Id = 2, Title = "Во все тяжкие",       Genre = "Драма",      Type = WatchType.Series, Status = WatchStatus.Watching, DateAdded = new DateTime(2026, 5, 3)  },
                new WatchItem { Id = 3, Title = "Дюна: часть вторая",  Genre = "Фантастика", Type = WatchType.Movie,  Status = WatchStatus.Planned,  DateAdded = new DateTime(2026, 5, 10) },
                new WatchItem { Id = 4, Title = "Очень странные дела", Genre = "Триллер",    Type = WatchType.Series, Status = WatchStatus.Planned,  DateAdded = new DateTime(2026, 5, 12) }
            );
        }
    }
}
