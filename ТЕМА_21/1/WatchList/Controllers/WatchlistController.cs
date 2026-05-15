using Microsoft.AspNetCore.Mvc;
using WatchList.Models;
using WatchList.Services;
using WatchList.ViewModels;

namespace WatchList.Controllers
{
    public class WatchlistController : Controller
    {
        private readonly IWatchlistService service;

        public WatchlistController(IWatchlistService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            var all = await service.GetAllAsync();
            ViewBag.Total = all.Count;
            var groups = all
                .GroupBy(i => i.Status)
                .OrderBy(g => g.Key)
                .ToList();
            return View(groups);
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpGet]
        public IActionResult Add() => View(new WatchlistItemViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(WatchlistItemViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var item = new WatchItem
            {
                Title = model.Title,
                Genre = model.Genre,
                Type = model.Type,
                Status = model.Status
            };
            await service.AddAsync(item);
            TempData["Message"] = $"\"{item.Title}\" добавлено в список";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await service.GetByIdAsync(id);
            if (item == null) return NotFound();
            var model = new WatchlistItemViewModel
            {
                Id = item.Id,
                Title = item.Title,
                Genre = item.Genre,
                Type = item.Type,
                Status = item.Status
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WatchlistItemViewModel model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var item = new WatchItem
            {
                Id = model.Id,
                Title = model.Title,
                Genre = model.Genre,
                Type = model.Type,
                Status = model.Status
            };
            if (!await service.UpdateAsync(item)) return NotFound();
            TempData["Message"] = "Запись обновлена";
            return RedirectToAction(nameof(Index));
        }

        [Route("Watchlist/MarkAsWatched/{id:int}")]
        public async Task<IActionResult> MarkAsWatched(int id)
        {
            if (!await service.MarkAsWatchedAsync(id)) return NotFound();
            TempData["Message"] = "Отмечено как просмотренное";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (await service.RemoveAsync(id))
                TempData["Message"] = "Запись удалена из списка";
            return RedirectToAction(nameof(Index));
        }
    }
}
