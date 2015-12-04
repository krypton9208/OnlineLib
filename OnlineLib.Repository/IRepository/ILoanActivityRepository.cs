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
        bool NewLoad(Guid libUserGuid, int bookid);
        bool ReturnLoad(Guid libUserGuid, int bookid);
        bool ExtendSheduledReturnData(Guid libUserGuid, int bookid);

    }
}
