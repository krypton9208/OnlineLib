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
    public class LibraryRepository : ILibraryRepository
    {
        private readonly OnlineLibDbContext _db;

        public LibraryRepository(OnlineLibDbContext db)
        {
            _db = db;
        }

        public LibUser GetUserByGuid(Guid id) => _db.Users.First(x => x.Id == id);

        public ICollection<Library> GetLibraryList => _db.Library.ToList();

        public Library GetLibraryById(int id)
        {
            return _db.Library.FirstOrDefault(x => x.Id == id);
        }

        public Library GetLibraryByName(string name)
        {
            return _db.Library.FirstOrDefault(x => x.Name == name);
        }

        public Guid GetUserGuidByEmail(string email) => _db.Users.First(x => x.Email == email).Id;

        public bool RemoveUserFromLibrary(int idlib, Guid iduser)
        {
            var library = _db.Library.First(x => x.Id == idlib);
            library.LibUsers.Remove(_db.Users.First(x => x.Id == iduser));
            var j = _db.Users.First(x => x.Id == iduser);
            j.Libraries.Remove(library);
            var t = j.Roles.First(x => x.WorkPlace.Id == library.Id);
            j.Roles.Remove(t);
            _db.Library.AddOrUpdate(library);
            _db.Users.AddOrUpdate(j);
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

        public bool AddLibrary(Library library, LibUser id)
        {
            if (library != null)
            {
                library.LibUsers.Add(id);
                id.Libraries.Add(library);
                id.Roles.Add(new LibUserRole() {RoleId = _db.Roles.First(x=>x.Name == "LibOwners").Id, UserId = id.Id, WorkPlace = library});
                _db.Users.AddOrUpdate(id);
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
                Library firstOrDefault = _db.Library.FirstOrDefault(x => x.Id == lib);
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

        public bool Subscribe(int lib, Guid id)
        {
            if (lib > 0 && id != Guid.Empty)
            {
                var library = _db.Library.First(x => x.Id == lib);
                var user = _db.Users.First(x => x.Id == id);
                library.LibUsers.Add(user);
                user.Libraries.Add(library);
                var g = _db.Roles.First(x => x.Name == "Readers");
                library.Workers.Add(new LibUserRole() { RoleId = g.Id, UserId = user.Id, WorkPlace = library });
                _db.Users.AddOrUpdate(user);
                _db.Library.AddOrUpdate(library);

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

        public bool UserSubscibeLibrary(int lib, Guid user)
        {
            if (_db.Users.First(x => x.Id == user).Libraries.Any(x => x.Id == lib)) return true;
            return false;
        }

        public bool IsWorker(int lib, Guid user)
        {
            var w = _db.Roles.First(x => x.Name == "Workers");
            var W = _db.Roles.First(x => x.Name == "Main_Workers");
            var o = _db.Roles.First(x => x.Name == "LibOwners");
            var libUserRole = _db.Users.First(x => x.Id == user).Roles.FirstOrDefault(d => d.WorkPlace.Id == lib);
            if (libUserRole != null && (libUserRole.RoleId == w.Id || libUserRole.RoleId == W.Id || libUserRole.RoleId == o.Id))
                return true;
            return false;
        }
    }
}
