using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.WebPages;
using OnlineLib.Models;
using OnlineLib.Repository.IRepository;
using OnlineLib.Repository.ViewModels;

namespace OnlineLib.Repository.Repository
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly OnlineLibDbContext _db;

        public LibraryRepository(OnlineLibDbContext db)
        {
            _db = db;
        }

        public string GetUserCode(Guid user)
        {
            string tt = _db.Users.First(x => x.Id == user).GetUserCode();
            if (tt.IsEmpty())
            {
                tt = GetUniqueKey();
                _db.Users.First(x => x.Id == user).UserCode = tt;
                try
                {
                    _db.SaveChanges();
                    return tt;
                }
                catch (Exception)
                {
                    return String.Empty;
                }
            }
            return tt;
        }
        private string GetUniqueKey()
        {
            int maxSize = 16;
            int minSize = 16;
            // ReSharper disable once RedundantAssignment
            char[] chars = new char[70];
            const string a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            chars = a.ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            int size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - minSize)]); }
            return result.ToString();
        }

        public string GetUserFirstAndSecondName(Guid user)
        {
            var t = _db.Users.First(x => x.Id == user);
            return t.Name + " " + t.Surname;
        }
        public LibUser GetUserByGuid(Guid id) => _db.Users.First(x => x.Id == id);

        public ICollection<Library> GetLibraryList => _db.Library.ToList();

        public Library GetLibraryById(int id)
        {
            return _db.Library.FirstOrDefault(x => x.Id == id);
        }


        public ProfiEditViewModel GetUserEditViewModel(Guid user)
        {
            LibUser tUser = _db.Users.FirstOrDefault(x => x.Id == user);
            if (tUser != null)
                return new ProfiEditViewModel()
                {
                    Id = tUser.Id,
                    Name = tUser.Name,
                    Surname = tUser.Surname

                };
            return new ProfiEditViewModel();
        }

        public bool UpdateUserEditVieModel(ProfiEditViewModel model)
        {
            var tuser = _db.Users.First(x => x.Id == model.Id);
            tuser.Name = model.Name;
            tuser.Surname = model.Surname;

            _db.Users.AddOrUpdate(tuser);
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

        public bool IsUserWithThisEMail(string email)
        {
            if (_db.Users.Count(x => x.Email == email) > 0)
                return true;
            return false;
        }

        public Guid GetUserGuidFromEmail(string email) => _db.Users.First(x => x.Email == email).Id;

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
            }
            return true;
        }

        public bool AddLibrary(Library library, LibUser id)
        {
            if (library != null)
            {
                library.LibUsers.Add(id);
                id.Libraries.Add(library);
                id.Roles.Add(new LibUserRole() { RoleId = _db.Roles.First(x => x.Name == "LibOwners").Id, UserId = id.Id, WorkPlace = library });
                _db.Users.AddOrUpdate(id);
                _db.Library.Add(library);

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
                }
                return true;
            }
            return false;
        }

        public void Dispose()
        {
        }

        public bool IsWorker(string email, int lib)
        {
            return IsWorker(lib, GetUserGuidFromEmail(email));
        }

        public bool UserSubscibeLibrary(int lib, Guid user)
        {
            if (_db.Users.First(x => x.Id == user).Libraries.Any(x => x.Id == lib) && !IsLibOwner(user, lib))
            {
                var library = _db.Library.First(x => x.Id == lib);
                library.LibUsers.Remove(_db.Users.First(x => x.Id == user));
                _db.Users.First(x => x.Id == user).Libraries.Remove(library);
                if (library.Workers.First(x => x.UserId == user) != null)
                    library.Workers.Remove(_db.Users.First(x => x.Id == user).Roles.First(x => x.WorkPlace.Id == lib));
                _db.Users.AddOrUpdate(_db.Users.First(x => x.Id == user));
                _db.Library.AddOrUpdate(library);
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
            return true;
        }

        public bool IsLibOwner(Guid user, int lib)
        {
            if (user != Guid.Empty && lib != 0)
            {
                if (_db.Users.First(x => x.Id == user).Roles.Count(x => x.WorkPlace.Id == lib && x.RoleId == _db.Roles.First(w => w.Name == "LibOwners").Id) >0)
                    return true;
                return false;
            }
            return false;
        }

        public bool IsWorker(int lib, Guid user)
        {
            var w = _db.Roles.First(x => x.Name == "Workers");
            var mw = _db.Roles.First(x => x.Name == "Main_Workers");
            var o = _db.Roles.First(x => x.Name == "LibOwners");
            if (_db.Users.First(x => x.Id == user).Roles.Count(d => d.WorkPlace.Id == lib) > 0)
            {
                var libUserRole = _db.Users.First(x => x.Id == user).Roles.First(d => d.WorkPlace.Id == lib);
                return libUserRole != null && (libUserRole.RoleId == w.Id || libUserRole.RoleId == mw.Id || libUserRole.RoleId == o.Id);
            }
            return false;
        }

    }
}
