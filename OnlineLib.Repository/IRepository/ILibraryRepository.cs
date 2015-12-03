using System;
using System.Collections.Generic;
using OnlineLib.Models;

namespace OnlineLib.Repository.IRepository
{
    public interface ILibraryRepository
    {
        ICollection<Library> GetLibraryList { get; }
        LibUser GetUserByGuid(Guid id);
        Library GetLibraryById(int id);
        Library GetLibraryByName(string name);
        bool RemoveUserFromLibrary(int idlib, Guid iduser);
        bool AddLibrary(Library library, LibUser id);
        Guid GetUserGuidByEmail(string email);
        bool UpdateLibrary(Library library);
        bool RemoveLibrary(Library library);
        ICollection<Book> GetAllBooks(int lib);
        bool AddBook(int lib, Book book);
        bool RemoveBook(int lib, Book book);
        void Dispose();
        void SaveChanges();
    }
}