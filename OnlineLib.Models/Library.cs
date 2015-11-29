using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLib.Models
{
    public class Library
    {
        public int Id { get; set; }

        [StringLength(200, ErrorMessage = "Nazwa biblioteki musi mieć wiecej niż 5 znaków." ,MinimumLength = 5)]
        public string Name { get; set; }

        [Display(Name = "Zdjęcie: ")]
        public string Photo { get; set; }

        public LibUser Owner { get; set; }

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

    public class LibraryMap : EntityTypeConfiguration<Library>
    {
        public LibraryMap()
        {
            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).IsRequired().HasMaxLength(200).HasColumnName("Nazwa: ");
            Property(x => x.Photo).IsOptional().HasMaxLength(200).HasColumnName("Zdjęcie: ");
            Property(x => x.Text).IsOptional().HasMaxLength(1500).HasColumnName("Tekst: ");
            //Property(x => x.Owner).IsOptional().HasColumnName("Owner: ");

            HasOptional(x => x.Books).WithMany().Map(t => t.MapKey("BooksId")).WillCascadeOnDelete(true);
            HasOptional(x => x.Readers).WithMany().Map(t => t.MapKey("ReadersId")).WillCascadeOnDelete(false);
            HasOptional(x => x.Workers).WithMany().Map(t => t.MapKey("WorkersId")).WillCascadeOnDelete(false);

        }
    }
}
