using System.ComponentModel.DataAnnotations;

namespace WatchList.Models
{
    public enum WatchType
    {
        Movie,
        Series
    }

    public enum WatchStatus
    {
        Planned,
        Watching,
        Watched
    }

    public class WatchItem
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Жанр")]
        public string Genre { get; set; } = string.Empty;

        [Display(Name = "Тип")]
        public WatchType Type { get; set; }

        [Display(Name = "Статус")]
        public WatchStatus Status { get; set; }

        [Display(Name = "Добавлено")]
        public DateTime DateAdded { get; set; }
    }
}
