using Microsoft.AspNetCore.Mvc;
using WatchList.Models;

namespace WatchList.Controllers
{
    public class WatchController : Controller
    {
        private static readonly List<WatchItem> items = new()
        {
            new WatchItem { Id = 1, Title = "Интерстеллар", Type = WatchType.Movie, Status = WatchStatus.Watched, DateAdded = DateTime.Now.AddDays(-30) },
            new WatchItem { Id = 2, Title = "Во все тяжкие", Type = WatchType.Series, Status = WatchStatus.Watching, DateAdded = DateTime.Now.AddDays(-10) },
            new WatchItem { Id = 3, Title = "Дюна: часть вторая", Type = WatchType.Movie, Status = WatchStatus.Planned, DateAdded = DateTime.Now.AddDays(-3) },
            new WatchItem { Id = 4, Title = "Очень странные дела", Type = WatchType.Series, Status = WatchStatus.Planned, DateAdded = DateTime.Now.AddDays(-1) }
        };

        public IActionResult Index(WatchStatus? status)
        {
            ViewBag.CurrentStatus = status;
            var list = status.HasValue
                ? items.Where(i => i.Status == status.Value).ToList()
                : items.ToList();
            return View(list);
        }

        public IActionResult Details(int id)
        {
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpGet]
        public IActionResult Add() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(WatchItem item)
        {
            if (!ModelState.IsValid) return View(item);

            item.Id = items.Count == 0 ? 1 : items.Max(i => i.Id) + 1;
            item.DateAdded = DateTime.Now;
            items.Add(item);
            return RedirectToAction(nameof(Index));
        }

        [Route("Watch/MarkAsWatched/{id:int}")]
        public IActionResult MarkAsWatched(int id)
        {
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item == null) return NotFound();
            item.Status = WatchStatus.Watched;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var item = items.FirstOrDefault(i => i.Id == id);
            if (item != null) items.Remove(item);
            return RedirectToAction(nameof(Index));
        }
    }
}
