using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineLib.Repository.IRepository;

namespace OnlineLib.App.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBooksRepository _booksRepository;
        // GET: Books
        [Route("{lib}/Books")]
        public ActionResult Index(int lib)
        {
            
            return View(_booksRepository.GetBooks(lib));
        }
    }
}