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
        public virtual Guid LibUsers { get; set; }
        public string Text { get; set; }
    }

    public class LibraryLibUser
    {
        public int LibraryId { get; set; }
        public Guid LibUserId { get; set; }

        public Library Library { get; set; }
        public LibUser LibUser { get; set; }

        public LibraryLibUser(Library library, LibUser libUser)
        {
            LibraryId = library.Id;
            LibUserId = libUser.Id;
        }
    }

    public class LibraryLibUserConfiguration : EntityTypeConfiguration<LibraryLibUser>
    {
        public LibraryLibUserConfiguration()
        {
            HasKey(x => new {x.LibraryId, x.LibUserId });
            HasRequired(x => x.Library).WithMany().HasForeignKey(x => x.LibraryId);
            HasRequired(x => x.LibUser).WithMany().HasForeignKey(x => x.LibUserId);
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
            
            
            HasRequired(x => x.Address).WithMany().HasForeignKey(t => t.AdresId);
            
            ToTable("Library");
        }
    }
}
