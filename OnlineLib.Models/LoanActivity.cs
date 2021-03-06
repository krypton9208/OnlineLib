﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace OnlineLib.Models
{
    public class LoanActivity
    {
        public Guid Id { get; set; }
        public DateTime LoanData { get; set; }
        public DateTime ReturnedData { get; set; }
        public DateTime ScheduledReturnData { get; set; }
        public bool Returned { get; set; }
        public string Text { get; set; }

        public virtual Book Book { get; set; }
        public virtual LibUser LibUser { get; set; }

        public LoanActivity() { }

        public LoanActivity(Book book, LibUser libuser, DateTime loandata, DateTime returneddata, DateTime sche, bool returner)
        {
            Book = book;
            LibUser = libuser;
            LoanData = loandata;
            ReturnedData = returneddata;
            ScheduledReturnData = sche;
            Returned = returner;
        }
    }

    public class LoanActivityConfiguration : EntityTypeConfiguration<LoanActivity>
    {
        public LoanActivityConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.LoanData).HasColumnType("datetime2").IsRequired();
            Property(x => x.ScheduledReturnData).HasColumnType("datetime2").IsRequired();
            Property(x => x.ReturnedData).HasColumnType("datetime2").IsOptional();
            Property(x => x.Returned).IsOptional();
            Property(x => x.Text).IsOptional().HasMaxLength(150);
            HasRequired(x => x.LibUser).WithMany(x => x.BookedBooks).Map(e => e.MapKey("LibUserId"));
            HasRequired(x => x.Book).WithOptional(x => x.LoadActivity).Map(e => e.MapKey("BookId"));

        }
    }
}
