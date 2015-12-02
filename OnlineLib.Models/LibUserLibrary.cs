using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLib.Models
{
    public class LibUserLibrary
    {
        public Guid LibUserId { get; set; }
        public int LibraryId { get; set; }

        public virtual LibUser LibUser { get; set; }
        public virtual Library Library { get; set; }
    }

    public class LibUserLibraryConfiguration : EntityTypeConfiguration<LibUserLibrary>
    {
        internal LibUserLibraryConfiguration()
        {
            HasKey(x => new {x.LibUserId, x.LibraryId});


            HasRequired(x => x.LibUser)
                .WithMany(t => t.LibUserLibraries)
                .Map(d => d.MapKey("LibUser"));
            HasRequired(x => x.Library)
                .WithMany(t => t.LibUserLibraries)
                .Map(d => d.MapKey("Library"));

            ToTable("LibUserLibrary");

        }
    }
}
