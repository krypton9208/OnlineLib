using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using OnlineLib.Repository.IRepository;
using Microsoft.AspNet.Identity.Owin;
using OnlineLib.App.ViewModels;
using OnlineLib.Models;

namespace OnlineLib.App.Controllers
{
    [Authorize]
    public class LibraryManagerController : Controller
    {
        private readonly ILibManagerRepository _libManagerRepository;
        private readonly ILibraryRepository _libraryRepository;
        private LibSignInManager _signInManager;
        private LibUserManager _userManager;
        public LibraryManagerController(LibUserManager userManager, LibSignInManager signInManager, ILibManagerRepository libManagerRepository, ILibraryRepository libraryRepository)
        {
            _libManagerRepository = libManagerRepository;
            _libraryRepository = libraryRepository;
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public LibSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<LibSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public LibUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<LibUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: LibraryManager
        [Route("{lib}/Manager/Workers")]
        public ActionResult Index(int lib)
        {
            if (_libManagerRepository.IsLibOwner(Guid.Parse(User.Identity.GetUserId()), lib))
            {
                ViewBag.LibOwner = true;
                ViewBag.Library = lib;
                return View(_libManagerRepository.GetWorkers(lib));
            }
            ViewBag.LibOwner = false;
            return View("Error");
        }
     
        [Route("{lib}/Manager/Workers/Add")]
        public ActionResult AddWorker(int lib)
        {
            if (_libManagerRepository.IsLibOwner(Guid.Parse(User.Identity.GetUserId()), lib))
            {
                ViewBag.Library = lib;
                return View();
            }
            else return View("Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{lib}/Manager/Workers/Add")]
        public async System.Threading.Tasks.Task<ActionResult> AddWorker(int lib, AddWorkerViewModel model)
        {
            var user = new LibUser()
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname
            };
            var password = Membership.GeneratePassword(8, 1) + "0";
            var result = await UserManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                _libManagerRepository.ChangeUserToWorker(lib, user.Id);

                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.EmailService.SendAsync(new IdentityMessage()
                {
                    Body = "Your templary password : " + password + "<br>Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>",
                    Destination = user.Email,
                    Subject = "Confirm your account"
                });
                return RedirectToAction("Index", "LibraryManager", new { @lib = lib });
            }
            return View(model);
        }
        
        [Route("{lib}/Manager/Workers/EditWorker/{user}")]
        public ActionResult EditWorker(int lib, Guid user)
        {
            if (_libManagerRepository.IsLibOwner(Guid.Parse(User.Identity.GetUserId()), lib))
            {
                ViewBag.Library = lib;
                ViewBag.Roles = _libManagerRepository.GetRoles();
                return View(_libManagerRepository.GetWorker(user,lib));
            }
            else return View("Error");
        }

        [Route("{lib}/Manager/Workers/EditWorker/{user}")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditWorker(int lib, Repository.ViewModels.ListWorkersViewModel model)
        {
            if (_libManagerRepository.IsLibOwner(Guid.Parse(User.Identity.GetUserId()), lib))
            {

                if ( _libManagerRepository.ChangeUserRange(lib, model.Id, model.RoleName))
                return RedirectToAction("Index", "LibraryManager", new { @lib = lib });

            }
            return View(_libManagerRepository.GetWorker(model.Id, lib));
        }

        [Route("{lib}/Manager/TextEditor")]
        public ActionResult EditLibraryText(int lib)
        {
            if (_libManagerRepository.IsLibOwner(Guid.Parse(User.Identity.GetUserId()), lib))
            {
               
                return View(_libraryRepository.GetLibraryById(lib));
            }
            return View("Error");
        }

        [Route("{lib}/Manager/TextEditor")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditLibraryText(int lib, Library model)
        {
            if (_libManagerRepository.IsLibOwner(Guid.Parse(User.Identity.GetUserId()), lib))
            {
                var d = _libraryRepository.GetLibraryById(lib);
                d.Text = model.Text;
                if (_libraryRepository.UpdateLibrary(d))
                    return RedirectToAction("Index", "Library", new {@lib = lib});
            }
            return View("Error");
        }
        [Route("{lib}/Manager/SearchWorker")]
        public ActionResult SearchWorker(int lib)
        {
            ViewBag.Library = lib;
            return View();
        }
        [Route("{lib}/Manager/SearchWorker")]
        [HttpPost]
        public ActionResult SearchWorker(int lib, string email)
        {
            if (_libraryRepository.IsUserWithThisEMail(email))
            {
                if (_libManagerRepository.ChangeUserToWorker(lib, _libraryRepository.GetUserGuidFromEmail(email)))
                    return RedirectToAction("Index", new {@lib = lib});
            }
            ViewBag.StatusMessage = "email not found";
            ViewBag.Library = lib;
            return View();
        }
    }
}