using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OnlineLib.Repository.IRepository;

namespace OnlineLib.App.Controllers
{
    [Authorize]
    public class LoansController : Controller
    {
        private readonly ILoanActivityRepository _loanActivityRepository;

        public LoansController(ILoanActivityRepository _repo)
        {
            _loanActivityRepository = _repo;
        }
        // GET: Loans
        [Route("{lib}/Loans/NewLoan")]

        public ActionResult NewLoan(int lib)
        {
            if (_loanActivityRepository.CanUserLoansBooks(Guid.Parse(User.Identity.GetUserId()), lib))
            {
                ViewBag.Library = lib;
                return View();
            }
            return View("Error");

        }

        [Route("{lib}/Loans/NewLoan")]
        [HttpPost]
        public ActionResult NewLoan(int? lib, string bookid, string libUserGuid)
        {
            if (lib != 0 && bookid != string.Empty && libUserGuid != string.Empty)
            {
                if (_loanActivityRepository.NewLoad(libUserGuid, bookid))
                {
                    return RedirectToAction("Index", "Books", new { @lib = (int)lib });
                }
                return View();
            }
            return View();
        }

        [Route("{lib}/Loans/ReturnBook")]
        public ActionResult ReturnBook(int lib)
        {
            ViewBag.Library = lib;
            return View();
        }

        [Route("{lib}/Loans/ReturnBook")]
        [HttpPost]
        public ActionResult ReturnBook(int lib, string bookid, string libUserGuid)
        {
            if (lib != 0 && bookid != string.Empty && libUserGuid != string.Empty)
            {
                if (_loanActivityRepository.ReturnLoad(libUserGuid, bookid))
                {
                    return RedirectToAction("Index", "Books", new { @lib = lib });
                }
                return View();
            }
            return View();
        }
    }
}