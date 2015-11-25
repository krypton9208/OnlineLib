using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineLib.App.Models
{
    public class Address
    {
        [Key]
        [Display(Name = "Id: ")]
        public  Guid Id { get; set; }

        [Display(Name = "Kraj: ")]
        [Required(ErrorMessage = "Podaj Państwo ", AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "Nazwa kraju musi być dłóższa niż 3 znaki", MinimumLength = 3)]
        public string Contry { get; set; }

        [Display(Name = "Miasto: ")]
        [Required(ErrorMessage = "Podaj Miasto ", AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "Nazwa miasta musi być dłóższa niż 3 znaki", MinimumLength = 3)]
        public string City { get; set; }

        [Display(Name = "Ulica: ")]
        [Required(ErrorMessage = "Podaj Ulice ", AllowEmptyStrings = false)]
        [StringLength(150, ErrorMessage = "Nazwa ulicy musi być dłóższa niż 3 znaki", MinimumLength = 3)]
        public string Street { get; set; }

        [Display(Name = "Numer budynku: ")]
        [Required(ErrorMessage = "Numer budynku jest wymagany: ", AllowEmptyStrings = false)]
        [StringLength(6, MinimumLength = 1)]
        public string Number { get; set; }


        [Display(Name = "Kod Pocztowy: ")]
        [Required(ErrorMessage = "Podaj Kod pocztowy: ", AllowEmptyStrings = false)]
        [StringLength(6, ErrorMessage = "Kod pocztowy jet w formacie XX-XXX", MinimumLength = 6)]
        public string PostCode { get; set; }
    }
}