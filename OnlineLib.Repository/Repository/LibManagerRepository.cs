using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineLib.Models;
using OnlineLib.Repository.IRepository;

namespace OnlineLib.Repository.Repository
{
    public class LibManagerRepository : ILibManagerRepository
    {
        private readonly OnlineLibDbContext _db;
        public LibManagerRepository(OnlineLibDbContext db)
        {
            _db = db;
        }
        public bool ChangeUserToWorker(int lib, Guid user)
        {
            throw new NotImplementedException();
        }

        public bool ChangeUserRange(int lib, Guid user, string Role)
        {
            if (lib != 0 && user != Guid.Empty)
            {
                if (_db.Roles.First(x => x.Name == Role) != null)
                {
                    _db.Users.First(x => x.Id == user).Roles.First(x => x.WorkPlace.Id == lib).RoleId =
                        _db.Roles.First(x => x.Name == Role).Id;
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
            return false;
        }

        public bool IsLibOwner(Guid user, int lib)
        {
            if (user != Guid.Empty && lib != 0)
            {
                if (_db.Users.First(x=>x.Id == user).Roles.Count(x => x.WorkPlace.Id == lib && x.RoleId == _db.Roles.First(w=>w.Name == "LibOwners").Id) == 1)
                    return true;
                return false;
            }
            return false;
        }

        public ICollection<LibUser> GetWorkers(int lib)
        {
            if (lib != 0)
            {
                var w = _db.Roles.First(x => x.Name == "Workers");
                var W = _db.Roles.First(x => x.Name == "Main_Workers");
                return
                    _db.Users.Where(
                        x =>
                            x.Roles.FirstOrDefault(d => d.WorkPlace.Id == lib).RoleId == w.Id ||
                            x.Roles.FirstOrDefault(o => o.WorkPlace.Id == lib).RoleId == W.Id).ToList();
            }
            return null;
        }
    }
}
