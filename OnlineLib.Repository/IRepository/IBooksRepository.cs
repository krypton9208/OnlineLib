using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineLib.Models;

namespace OnlineLib.Repository.IRepository
{
    public interface IBooksRepository
    {
        ICollection<Book> GetBooks(Guid lib);
        Book GetBookById(int id);
        bool Add(Book book, Guid lib);
        bool Update(Book book);
        bool Remove(Book book, Guid lib);
        bool SaveChanges();
        bool IsBooked(int id);
        //bool Dispose(bool disposing);
    }
}
