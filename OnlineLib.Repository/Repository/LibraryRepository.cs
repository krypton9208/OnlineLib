using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineLib.Models;
using OnlineLib.Repository.IRepository;

namespace OnlineLib.Repository.Repository
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly OnlineLibDbContext _db;

        public LibraryRepository(OnlineLibDbContext db)
        {
            _db = db;
        }

        public ICollection<Library> GetLibraryList => _db.Library.ToList();

        public Library GetLibraryById(int id)
        {
            return _db.Library.FirstOrDefault(x => x.Id == id);
        }

        public Library GetLibraryByName(string name)
        {
            return _db.Library.FirstOrDefault(x => x.Name == name);
        }

        public bool AddLibrary(Library library)
        {
            if (library != null)
            {
                _db.Library.Add(library);
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

        public bool UpdateLibrary(Library library)
        {
            if (library != null)
            {
                _db.Entry(library).State = EntityState.Modified;
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

        public bool RemoveLibrary(Library library)
        {
            if (library != null)
            {
                _db.Library.Remove(library);
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

        //public ICollection<LibUser> GetEmployees(int lib)
        //{
        //    Library firstOrDefault = _db.Library.FirstOrDefault(x => x.Id == lib);
        //    return firstOrDefault?.Workers;
        //}

        


        public ICollection<Book> GetAllBooks(int lib)
        {
            Library firstOrDefault = _db.Library.FirstOrDefault(x => x.Id == lib);
            return firstOrDefault?.Books;
        }

        public bool AddEmployee(int lib, LibUser employee)
        {
            if (lib > 0 && employee != null)
            {
                Library firstOrDefault = _db.Library.FirstOrDefault(x=>x.Id == lib);
              //  if (firstOrDefault?.Workers == null) firstOrDefault.Workers = new List<LibUser>();
              //  firstOrDefault?.Workers.Add(employee);
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

        public bool RemoveEmployee(int lib, LibUser employee)
        {
            if (lib > 0 && employee != null)
            {
                Library firstOrDefault = _db.Library.FirstOrDefault(x => x.Id == lib);
                //firstOrDefault?.Workers.Remove(employee);
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

        public bool AddReaders(int lib, LibUser readers)
        {
            if (lib > 0 && readers != null)
            {
                Library firstOrDefault = _db.Library.FirstOrDefault(x => x.Id == lib);
               // if (firstOrDefault?.Readers == null) firstOrDefault.Readers = new List<LibUser>();
                //firstOrDefault?.Readers.Add(readers);
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

        public bool RemoveReaders(int lib, LibUser readers)
        {
            if (lib > 0 && readers != null)
            {
                Library firstOrDefault = _db.Library.FirstOrDefault(x => x.Id == lib);
               // firstOrDefault?.Readers.Remove(readers);
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

        public bool AddBook(int lib, Book book)
        {
            if (lib > 0 && book != null)
            {
                Library firstOrDefault = _db.Library.FirstOrDefault(x => x.Id == lib);
                firstOrDefault?.Books.Add(book);
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

        public bool RemoveBook(int lib, Book book)
        {
            if (lib > 0 && book != null)
            {
                Library firstOrDefault = _db.Library.FirstOrDefault(x => x.Id == lib);
                firstOrDefault?.Books.Remove(book);
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

        public void Dispose()
        {
        }

        public void SaveChanges()
        {
            _db.SaveChanges(); 
        }
    }
}
