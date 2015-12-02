using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace OnlineLib.Models
{
    public class Library
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public Guid AdresId { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<LibUserLibrary>LibUserLibraries { get; set; }
        public string Text { get; set; }

        public Library()
        {
            Books = new HashSet<Book>();
            LibUserLibraries = new HashSet<LibUserLibrary>();
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
            Property(x => x.AdresId).IsOptional();

            HasMany(x => x.Books).WithOptional(d => d.Library).HasForeignKey(t => t.LibraryId);
            HasOptional(x => x.LibUserLibraries);
            HasOptional(x => x.Address).WithMany().HasForeignKey(t => t.AdresId).WillCascadeOnDelete(true);
            
            ToTable("Library");
        }
    }
}
