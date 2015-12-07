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
    public class LibraryManagerController : Controller
    {
        private readonly ILibManagerRepository _libManagerRepository;
        public LibraryManagerController(ILibManagerRepository _repo)
        {
            _libManagerRepository = _repo;
        }
        // GET: LibraryManager
        [Route("{lib}/Manager/Workers")]
        public ActionResult Index(int lib)
        {
            if (_libManagerRepository.IsLibOwner(Guid.Parse(User.Identity.GetUserId()), lib))
                return View(_libManagerRepository.GetWorkers(lib));
            return View("Error");
        }
    }
}