using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }

    public class LoanActivityConfiguration : EntityTypeConfiguration<LoanActivity>
    {
        public LoanActivityConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.LoanData).IsRequired();
            Property(x => x.ScheduledReturnData).IsRequired();
            Property(x => x.ReturnedData).IsOptional();
            Property(x => x.Returned).IsOptional();
            Property(x => x.Text).IsOptional().HasMaxLength(150);
            HasRequired(x => x.LibUser).WithMany(x => x.BookedBooks).Map(e => e.MapKey("LibUserId"));
            HasRequired(x => x.Book).WithOptional(x => x.LoadActivity).Map(e => e.MapKey("BookId"));

        }
    }
}
