using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using OnlineLib.Models;
using OnlineLib.Repository.IRepository;

namespace OnlineLib.Repository.Repository
{
    public class LoanActivityRepository : ILoanActivityRepository
    {
        private readonly OnlineLibDbContext _db;

        public LoanActivityRepository(OnlineLibDbContext db)
        {
            _db = db;
        }

        public bool CanUserLoansBooks(Guid user, int lib)
        {
            var o = _db.Roles.First(x => x.Name == "LibOwners").Id;
            var w = _db.Roles.First(x => x.Name == "Workers").Id;
            var m = _db.Roles.First(x => x.Name == "Main_Workers").Id;
            var h = _db.Users.First(x => x.Id == user);
            List<LibUserRole> k = h.Roles.Where(x => x.WorkPlace.Id == lib).ToList();
            if (k.Count >= 0)
            {
                return k.Any(libUserRole => (libUserRole.RoleId == w) || (libUserRole.RoleId == m) || (libUserRole.RoleId == o));
            }
            return false;
        }


        public bool NewLoad(string libUserGuid, string bookid)
        {
            if (libUserGuid != string.Empty && bookid != string.Empty)
            {
                var loan = new LoanActivity(_db.Book.FirstOrDefault(x => x.ShortId == bookid),
                    _db.Users.FirstOrDefault(x => x.UserCode == libUserGuid),
                    DateTime.Today, DateTime.MinValue, DateTime.Today.AddDays(30), false);
                LibUser user = _db.Users.FirstOrDefault(x => x.UserCode == libUserGuid);
                Book book = _db.Book.FirstOrDefault(x => x.ShortId == bookid);
                if (book != null && user != null)
                {
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
                    }
                    return true;
                }
                return false;

            }
            return false;
        }

        public bool ReturnLoad(string libUserGuid, string bookid)
        {
            if (libUserGuid != string.Empty && bookid != string.Empty)
            {
                var t = _db.LoanActivitie.First(x => x.LibUser.UserCode == libUserGuid && x.Book.ShortId == bookid);
                _db.Book.First(x => x.ShortId == bookid).Lended = false;
                _db.Book.First(x => x.ShortId == bookid).LoadActivity = null;
                _db.Users.First(x => x.UserCode == libUserGuid).BookedBooks.Remove(t);
                _db.LoanActivitie.Remove(t);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public List<LoanActivity> GetUserActiviciesList(Guid user)
        {
            return _db.LoanActivitie.Where(x => x.LibUser.Id == user && x.Returned == false).ToList();
        }

        public bool ExtendSheduledReturnData(int bookid)
        {
            if ( bookid != 0)
            {
                _db.LoanActivitie.First(x => x.Book.Id == bookid).ScheduledReturnData =
                    DateTime.Now.AddDays(30);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
