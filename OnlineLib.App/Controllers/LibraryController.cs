using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using OnlineLib.Models;
using OnlineLib.Repository.IRepository;
using Microsoft.AspNet.Identity;


namespace OnlineLib.App.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILibraryRepository _libraryRepository;


        public LibraryController(ILibraryRepository _repo)
        {
            _libraryRepository = _repo;
        }
        // GET: Library
        [Authorize]
        [HttpGet]
        [Route("Library/Create/{id:Guid?}")]
        public ActionResult Create(Guid? id)
        {
            ViewBag.User_Id = User.Identity.GetUserId();
            if (id == null) return RedirectToAction("Create", new { @id = ViewBag.User_Id });
            return View();
        }

        [Route("Library/Create/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Library library, HttpPostedFileBase file, Guid id)
        {


            if (file.FileName != null)
            {
                library.Photo = library.Name + ".jpg";

            }
            library.Address = new Address()
            {
                City = library.Address.City,
                Contry = library.Address.Contry,
                PostCode = library.Address.PostCode,
                Street = library.Address.Street,
                Number = library.Address.Number
            };
            try
            {
                if (_libraryRepository.AddLibrary(library, _libraryRepository.GetUserByGuid(id)))
                {
                    string fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Image"), library.Name + ".jpg");
                    file.SaveAs(path);
                    return RedirectToActionPermanent("Index", "Home");
                }
            }
            catch (Exception)
            {
                return View(library);
            }
            return View(library);
        }


    }
}