using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineLib.App.Models
{
    public class Book
    {
        [Key]
        [Display(Name = "Id: ")]
        public Guid Id { get; set; }

        [Display(Name = "Tytuł: ")]
        public string Title { get; set; }

        [Display(Name = "Autor: ")]
        public string Autor { get; set; }

        [Display(Name = "Wypożyczona: ")]
        public bool Lended { get; set; }

        [StringLength(13, ErrorMessage = "Numer isbn...", MinimumLength = 0)]
        public string ISBN { get; set; }

        [Required]
        [Display(Name = "W Bibliotece: ")]
        public virtual Library Library { get; set; }
    }
}