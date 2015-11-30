﻿using System;
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
        public string Name { get; set; }
        public string Photo { get; set; }
        public virtual LibUser Owner { get; set; }
        public virtual Address Address { get; set; }
        public virtual List<Book> Books { get; set; }
        public virtual List<LibUser> Workers { get; set; }
        public virtual List<LibUser> Readers { get; set; }
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


            HasOptional(x => x.Address);
            HasOptional(x => x.Owner);

            HasOptional(x => x.Books).WithMany().Map(t => t.MapKey("BooksId")).WillCascadeOnDelete(true);
            HasOptional(x => x.Readers).WithMany().Map(t => t.MapKey("ReadersId")).WillCascadeOnDelete(false);
            HasOptional(x => x.Workers).WithMany().Map(t => t.MapKey("WorkersId")).WillCascadeOnDelete(false);
            ToTable("Library");
        }
    }
}
