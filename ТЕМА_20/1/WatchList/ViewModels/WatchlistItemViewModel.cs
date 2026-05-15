using System.ComponentModel.DataAnnotations;
using WatchList.Models;

namespace WatchList.ViewModels
{
    public class WatchlistItemViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите название")]
        [StringLength(120, ErrorMessage = "Название не должно превышать 120 символов")]
        [Display(Name = "Название")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите жанр")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Жанр от 2 до 40 символов")]
        [RegularExpression(@"^[A-Za-zА-Яа-яЁё\s\-]+$", ErrorMessage = "Жанр может содержать только буквы")]
        [Display(Name = "Жанр")]
        public string Genre { get; set; } = string.Empty;

        [Display(Name = "Тип")]
        public WatchType Type { get; set; }

        [Display(Name = "Статус")]
        public WatchStatus Status { get; set; }
    }
}
