using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineLib.Models;
using OnlineLib.Repository.ViewModels;

namespace OnlineLib.Repository.IRepository
{
    public interface ILibManagerRepository
    {
        bool ChangeUserToWorker(int lib, Guid user);
        bool ChangeUserRange(int lib, Guid user, string role);
        bool IsLibOwner(Guid user, int lib);
        ICollection<ListWorkersViewModel> GetWorkers(int lib);
    }
}
