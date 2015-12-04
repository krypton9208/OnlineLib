using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineLib.Models;
using OnlineLib.Repository.IRepository;

namespace OnlineLib.Repository.Repository
{
     public class LoanActivityRepository : ILoanActivityRepository
     {
         public readonly OnlineLibDbContext _db;
         public LoanActivityRepository(OnlineLibDbContext db)
         {
             _db = db;
         }
        public bool NewLoad(Guid libUserGuid, int bookid)
         {
             if (libUserGuid != Guid.Empty && bookid != 0)
             {
                 LoanActivity loan = new LoanActivity()
                 {
                     Book = _db.Book.First(x=>x.Id == bookid),
                     LibUser = _db.Users.First(x=>x.Id == libUserGuid),
                     LoanData = DateTime.Today,
                     ScheduledReturnData = DateTime.Today.AddDays(30),
                     Returned = false
                 };
                 _db.LoanActivitie.Add(loan);
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

         public bool ReturnLoad(Guid libUserGuid, int bookid)
         {
             throw new NotImplementedException();
         }

         public bool ExtendSheduledReturnData(Guid libUserGuid, int bookid)
         {
             throw new NotImplementedException();
         }
    }
}
