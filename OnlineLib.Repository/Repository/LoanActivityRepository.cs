using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
                    Book = _db.Book.First(x => x.Id == bookid),
                    LibUser = _db.Users.First(x => x.Id == libUserGuid),
                    LoanData = DateTime.Today,
                    ReturnedData = DateTime.MinValue,
                    ScheduledReturnData = DateTime.Today.AddDays(30),
                    Returned = false
                };
                LibUser user = _db.Users.First(x => x.Id == libUserGuid);
                Book book = _db.Book.First(x => x.Id == bookid);
                loan.LibUser = user;
                loan.Book = book;
                book.Lended = true;
                book.LoadActivity = loan;
                user.BookedBooks.Add(loan);
                _db.Book.AddOrUpdate(book);
                _db.Users.AddOrUpdate(user);
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
            if (libUserGuid != Guid.Empty && bookid != 0)
            {
                var t = _db.LoanActivitie.First(x => x.LibUser.Id == libUserGuid && x.Book.Id == bookid);
                _db.Book.First(x => x.Id == bookid).Lended = false;
                _db.Book.First(x => x.Id == bookid).LoadActivity = null;
                _db.Book.First(x => x.Id == bookid).LoadActivity = null;
                _db.Users.First(x => x.Id == libUserGuid).BookedBooks.Remove(t);
                _db.LoanActivitie.Remove(t);
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

        public bool ExtendSheduledReturnData(Guid libUserGuid, int bookid)
        {
            if (libUserGuid != Guid.Empty && bookid != 0)
            {
                _db.LoanActivitie.First(x => x.LibUser.Id == libUserGuid && x.Book.Id == bookid).ScheduledReturnData =
                    DateTime.Now.AddDays(30);
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
    }
}
