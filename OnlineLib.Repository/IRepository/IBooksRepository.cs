using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineLib.Models;
using OnlineLib.Repository.Repository;
using OnlineLib.Repository.ViewModels;

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
        bool IsPrinted(Book book);
        BooksRepository.BookLabel GenerateCodeToPrint(int id);
        List<BooksRepository.BookLabel> GenerateCodeToPrint(List<int> lista);
        List<BookToPrint> BookToPrint(int lib);
        bool IsBooked(int id);
        //bool Dispose(bool disposing);
    }
}
