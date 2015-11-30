using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OnlineLib.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Autor { get; set; }
        public bool Lended { get; set; }
        public string Isbn { get; set; }
        public int LibraryId { get; set; }
        public virtual Library Library { get; set; }
    }

    public class BookConfiguration : EntityTypeConfiguration<Book>
    {
        public BookConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasMaxLength(100).IsRequired();
            Property(x => x.Autor).HasMaxLength(100).IsOptional();
            Property(x => x.Isbn).HasMaxLength(16).IsOptional();
            Property(x => x.Lended).IsOptional();
            HasRequired(x => x.Library).WithMany(t => t.Books).HasForeignKey(g => g.LibraryId);
            ToTable("Books");
        }
    }


}