using System.Collections.Generic;
using OnlineLib.Models;

namespace OnlineLib.Repository.IRepository
{
    public interface ILibraryRepository
    {
        ICollection<Library> GetLibraryList { get; }
        Library GetLibraryById(int id);
        Library GetLibraryByName(string name);
        bool AddLibrary(Library library);
        bool UpdateLibrary(Library library);
        bool RemoveLibrary(Library library);
        ICollection<LibUser> GetEmployees(int lib);
        ICollection<LibUser> GetReaders(int lib);
        ICollection<Book> GetAllBooks(int lib);
        bool AddEmployee(int lib, LibUser employee);
        bool RemoveEmployee(int lib, LibUser employee);
        bool AddReaders(int lib, LibUser employee);
        bool RemoveReaders(int lib, LibUser employee);
        bool AddBook(int lib, Book book);
        bool RemoveBook(int lib, Book book);
        void Dispose();
    }
}