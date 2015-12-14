using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Microsoft.AspNet.Identity;
using OnlineLib.Repository.IRepository;
using Microsoft.AspNet.Identity.Owin;
using OnlineLib.App.ViewModels;
using OnlineLib.Models;
using OnlineLib.Repository.ViewModels;

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
                return View(_libManagerRepository.GetWorkers(lib));
            return View("Error");
        }

        [Route("{lib}/Manager/Workers/Add")]
        public ActionResult AddWorker(int lib)
        {
            ViewBag.Library = lib;
            return View();
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
    }
}