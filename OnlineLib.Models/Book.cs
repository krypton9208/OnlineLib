using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OnlineLib.Models
{
    public class Book
    {
        [Key]
        [Display(Name = "Id: ")]
        public int Id { get; set; }

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

    public class BookMap : EntityTypeConfiguration<Book>
    {
        public BookMap()
        {
            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasMaxLength(100).IsRequired();
            Property(x => x.Autor).HasMaxLength(100).IsOptional();
            Property(x => x.ISBN).HasMaxLength(16).IsOptional();
            Property(x => x.Lended).IsOptional();


            ToTable("Books");
        }
    }


}