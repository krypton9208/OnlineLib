using System.Collections.Generic;
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
        BooksRepository.BookLabel GenerateCodeToPrint(int id);
        List<BooksRepository.BookLabel> GenerateCodeToPrint(List<int> lista);
        List<BookToPrint> BookToPrint(int lib);
        //bool Dispose(bool disposing);
    }
}
