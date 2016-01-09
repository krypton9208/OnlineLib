using System;
using System.Collections.Generic;
using OnlineLib.Models;

namespace OnlineLib.Repository.IRepository
{
    public interface ILoanActivityRepository
    {
        bool NewLoad(string libUserGuid, string bookid);
        bool ReturnLoad(string libUserGuid, string bookid);
        List<LoanActivity> GetUserActiviciesList(Guid user); 
        bool ExtendSheduledReturnData( int bookid);
        bool CanUserLoansBooks(Guid user, int lib);

    }
}
