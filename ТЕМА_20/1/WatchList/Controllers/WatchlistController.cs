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

        public IActionResult Index(WatchStatus? status)
        {
            ViewBag.CurrentStatus = status;
            ViewBag.Total = service.GetAll().Count;
            return View(service.GetAll(status));
        }

        public IActionResult Details(int id)
        {
            var item = service.GetById(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpGet]
        public IActionResult Add() => View(new WatchlistItemViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(WatchlistItemViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var item = new WatchItem
            {
                Title = model.Title,
                Genre = model.Genre,
                Type = model.Type,
                Status = model.Status
            };
            service.Add(item);
            TempData["Message"] = $"\"{item.Title}\" добавлено в список";
            return RedirectToAction(nameof(Index));
        }

        [Route("Watchlist/MarkAsWatched/{id:int}")]
        public IActionResult MarkAsWatched(int id)
        {
            if (!service.MarkAsWatched(id)) return NotFound();
            TempData["Message"] = "Отмечено как просмотренное";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (service.Remove(id))
                TempData["Message"] = "Запись удалена из списка";
            return RedirectToAction(nameof(Index));
        }
    }
}
