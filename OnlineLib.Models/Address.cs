using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OnlineLib.Models
{
    public class Address
    {
        public  Guid Id { get; set; }
        public string Contry { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string PostCode { get; set; }
        
    }

    public class AddresConfiguration : EntityTypeConfiguration<Address>
    {
        internal AddresConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id: ");
            Property(x => x.Contry).HasMaxLength(40).IsOptional().HasColumnName("Contry: ");
            Property(x => x.City).HasMaxLength(40).IsRequired().HasColumnName("City: ");
            Property(x => x.Street).HasMaxLength(60).IsRequired().HasColumnName("Street: ");
            Property(x => x.PostCode).HasMaxLength(6).IsRequired().HasColumnName("Post Code: ");
            Property(x => x.Number).HasMaxLength(7).IsRequired().HasColumnName("Number: ");

        }
    }
}