using System;
using System.Collections.Generic;
using System.Web.Mvc;
using OnlineLib.Repository.ViewModels;

namespace OnlineLib.Repository.IRepository
{
    public interface ILibManagerRepository
    {
        bool ChangeUserToWorker(int lib, Guid user);
        bool ChangeUserRange(int lib, Guid user, string role);
        bool IsLibOwner(Guid user, int lib);
        ICollection<ListWorkersViewModel> GetWorkers(int lib);
        ListWorkersViewModel GetWorker(Guid user, int lib);
        IEnumerable<SelectListItem> GetRoles();
    }
}
