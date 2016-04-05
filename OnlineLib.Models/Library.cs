using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OnlineLib.Models
{
    public class Library
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<LibUser> LibUsers { get; set; }
        public virtual ICollection<LibUserRole> Workers { get; set; }
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        public Library()
        {
            Books = new HashSet<Book>();
            LibUsers = new HashSet<LibUser>();
            Workers = new HashSet<LibUserRole>();
        }
    }
    
    public class LibraryConfiguration : EntityTypeConfiguration<Library>
    {
        public LibraryConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).IsRequired().HasMaxLength(200).HasColumnName("Nazwa: ");
            Property(x => x.Photo).IsOptional().HasMaxLength(200).HasColumnName("Zdjęcie: ");
            Property(x => x.Text).IsOptional().HasMaxLength(1500).HasColumnName("Tekst: ");
            HasOptional(x => x.LibUsers);
            HasOptional(x => x.Workers);
            HasOptional(x => x.Address).WithRequired(x => x.Library);
            ToTable("Library");
        }
    }
}
