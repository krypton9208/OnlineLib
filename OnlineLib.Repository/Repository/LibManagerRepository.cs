using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Windows.Forms;
using OnlineLib.Models;
using OnlineLib.Repository.ViewModels;
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
            if (ChangeUserRange(lib, user, "Workers"))
                return true;
            return false;
        }



        public bool ChangeUserRange(int lib, Guid user, string role)
        {
            if (lib != 0 && user != Guid.Empty)
            {
                if (_db.Roles.First(x => x.Name == role) != null)
                {
                    if (_db.Users.First(x => x.Id == user).Roles.Count(x => x.WorkPlace.Id == lib) != 0)
                    {
                        _db.Users.First(x => x.Id == user)
                            .Roles.Remove(_db.Users.First(x => x.Id == user).Roles.First(x => x.WorkPlace.Id == lib));
                        _db.Users.First(x => x.Id == user)
                            .Roles.Add(new LibUserRole()
                            {
                                RoleId = _db.Roles.First(x => x.Name == role).Id,
                                UserId = user,
                                WorkPlace = _db.Library.FirstOrDefault(x => x.Id == lib)
                            });
                    }
                    else
                    {
                        _db.Users.First(x => x.Id == user)
                            .Roles.Add(new LibUserRole()
                            {
                                RoleId = _db.Roles.First(x => x.Name == role).Id,
                                UserId = user,
                                WorkPlace = _db.Library.FirstOrDefault(x => x.Id == lib)
                            });

                    }

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
                if (
                    _db.Users.First(x => x.Id == user)
                        .Roles.Count(
                            x => x.WorkPlace.Id == lib && x.RoleId == _db.Roles.First(w => w.Name == "LibOwners").Id) ==
                    1)
                    return true;
                return false;
            }
            return false;
        }


        public ICollection<ListWorkersViewModel> GetWorkers(int lib)
        {
            if (lib != 0)
            {
                ICollection<ListWorkersViewModel> list = new HashSet<ListWorkersViewModel>();
                var w = _db.Roles.First(x => x.Name == "Workers");
                var W = _db.Roles.First(x => x.Name == "Main_Workers");

                var workers = _db.Users.Where(
                    x =>
                        x.Roles.FirstOrDefault(d => d.WorkPlace.Id == lib).RoleId == w.Id ||
                        x.Roles.FirstOrDefault(o => o.WorkPlace.Id == lib).RoleId == W.Id).ToList();
                foreach (LibUser user in workers)
                {
                    var role = "";
                    role = user.Roles.Count(x => x.WorkPlace.Id == lib && x.RoleId == w.Id) > 0
                        ? "Workers"
                        : "Main_Workers";
                    list.Add(new ListWorkersViewModel()
                    {
                        Id = user.Id,
                        Name = user.Name + " " + user.Surname,
                        RoleName = role
                    });
                }
                return list;
            }
            return null;
        }

        public ListWorkersViewModel GetWorker(Guid user, int lib)
        {
            var p = _db.Users.First(x => x.Id == user);
            var w = _db.Roles.First(x => x.Name == "Workers");
            var role = "";

            role = p.Roles.Count(x => x.WorkPlace.Id == lib && x.RoleId == w.Id) > 0 ? "Workers" : "Main_Workers";


            var tem = new ListWorkersViewModel()
            {
                Id = p.Id,
                Name = p.Name + " " + p.Surname,
                RoleName = role
            };
            return tem;

        }


        public IEnumerable<SelectListItem> GetRoles()
        {

            var dd = new List<SelectListItem>();
            foreach (var role in _db.Roles)
            {
                if (role.Name == "Workers" || role.Name == "Main_Workers")
                    dd.Add(new SelectListItem() {Text = role.Name, Value = role.Name});
            }
            return dd;

        }
    }
}
