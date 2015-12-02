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
        ICollection<Book> GetBooks(int lib);
        Book GetBookById(int id);
        bool Add(Book book, int lib);
        bool Update(Book book);
        bool Remove(Book book, int lib);
        bool SaveChanges();
        bool IsBooked(int id);
        //bool Dispose(bool disposing);
    }
}
