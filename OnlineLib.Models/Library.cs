using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLib.Models
{
    public class Library
    {
        [Key]
        [Display(Name = "Id: ")]
        public Guid Id { get; set; }

        [Display(Name = "Biblioteka: ")]
        [StringLength(200, ErrorMessage = "Nazwa biblioteki musi mieć wiecej niż 5 znaków." ,MinimumLength = 5)]
        public string Name { get; set; }

        [Display(Name = "Zdjęcie: ")]
        public byte Photo { get; set; }


        [Display(Name = "Adress: ")]
        public virtual Address _Address { get; set; }
        
        [Display(Name = "Ksiazki: ")]
        public virtual ICollection<Book> Books { get; set; }

        [Display(Name = "Pracownicy: ")]
        public virtual ICollection<LibUser> Workers { get; set; }

        [Display(Name = "Czytelnicy: ")]
        public virtual ICollection<LibUser> Readers { get; set; }


        [Display(Name = "Tekst: ")]
        [StringLength(1500, MinimumLength = 0)]
        public string Text { get; set; }
    }
}
