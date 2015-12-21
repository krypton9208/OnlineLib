using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineLib.Models;

namespace OnlineLib.Repository.IRepository
{
    public interface ILoanActivityRepository
    {
        bool NewLoad(string libUserGuid, string bookid);
        bool ReturnLoad(string libUserGuid, string bookid);
        bool ExtendSheduledReturnData(Guid libUserGuid, int bookid);
        bool CanUserLoansBooks(Guid user, int lib);

    }
}
