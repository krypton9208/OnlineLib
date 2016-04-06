using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FluentValidation;
using FluentValidation.Attributes;

namespace OnlineLib.Models
{
    [Validator(typeof(BookValidator))]
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Autor { get; set; }
        public bool Lended { get; set; }
        //[StringLength(16, ErrorMessage = "ISBN, Format XXXX-XXXX-XX, maksymalna ilość znakaów to 16.")]
        public string Isbn { get; set; }
        public int LibraryId { get; set; }
        public virtual Library Library { get; set; }
        public string ShortId { get; set; } 

        public virtual LoanActivity LoadActivity { get; set; }
    }

    public class BookConfiguration : EntityTypeConfiguration<Book>
    {
        public BookConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasMaxLength(100).IsRequired().HasColumnName("Title: ");
            Property(x => x.Autor).HasMaxLength(100).IsOptional().HasColumnName("Author: ");
            Property(x => x.Isbn).HasMaxLength(16).IsOptional().HasColumnName("Isbn: ");
            Property(x => x.Lended).IsOptional().HasColumnName("Lended: ");
            Property(x => x.ShortId).IsOptional().HasColumnName("ShortGuid: ");
            HasRequired<Library>(x => x.Library).WithMany(d => d.Books).HasForeignKey(w => w.LibraryId);
            ToTable("Books");
        }
    }

    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(book => book.Title).Length(3, 100).WithMessage("Please specify title");
            RuleFor(book => book.Autor).Length(3, 100).WithMessage("Please specify a autor of this book.");
            RuleFor(book => book.Isbn).Length(0,16).Matches("(ISBN[-]*(1[03])*[ ]*(: ){0,1})*(([0-9Xx][- ]*){13}|([0-9Xx][- ]*){10})").WithMessage("ISBN, Format XXXX-XXXX-XX, max length is equal 16 characters.");
        }
    }
}