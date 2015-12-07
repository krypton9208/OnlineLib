using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineLib.Models;

namespace OnlineLib.Repository.IRepository
{
    public interface ILibManagerRepository
    {
        bool ChangeUserToWorker(int lib, Guid user);
        bool ChangeUserRange(int lib, Guid user, string Role);
        bool IsLibOwner(Guid user, int lib);
        ICollection<LibUser> GetWorkers(int lib);
    }
}
