using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineLib.Models;
using OnlineLib.Repository.IRepository;

namespace OnlineLib.Repository.Repository
{
    public class BooksRepository : IBooksRepository
    {
        private readonly OnlineLibDbContext _db = new OnlineLibDbContext();

        public ICollection<Book> GetBooks(int lib)
        {
            return _db.Library.First(x => x.Id == lib).Books;
        }

        public Book GetBookById(int id)
        {
            return _db.Book.First(x => x.Id == id);
        }

        public bool Add(Book book, int lib)
        {
            if (book != null && lib != 0)
            {
                book.Library = _db.Library.First(x => x.Id == lib);
                _db.Book.Add(book);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool Update(Book book)
        {
            if (book != null)
            {
                var dd = _db.Book.First(x => x.Id == book.Id);
                dd.Autor = book.Autor;
                dd.Title = book.Title;
                dd.Isbn = book.Isbn;
                dd.Lended = book.Lended;
                dd.LoadActivity = book.LoadActivity;
                _db.Entry(dd).State = EntityState.Modified;
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                    throw;
                }
                return true;
            }
            return false;
        }

        public bool Remove(Book book, int lib)
        {
            if (!book.Lended)
            {
                var d = _db.Book.First(x => x.Id == book.Id);
                _db.Library.First(x => x.Id == lib).Books.Remove(d);
                _db.Book.Remove(d);
                try
                {
                    _db.SaveChanges();

                }
                catch (Exception)
                {
                    return false;
                    throw;
                }
                return true;
            }
            return false;
        }

        public bool SaveChanges()
        {
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return true;
        }

        public bool IsBooked(int id)
        {
            return _db.Book.First(x => x.Id == id).Lended;
        }

        //public override bool Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
