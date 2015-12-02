using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineLib.Models;
using OnlineLib.Repository.IRepository;
using System.Collections.Generic;
using System.Web.Routing;


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
        [Route("Library/Create/{id}")]
        public ActionResult Create(Guid id)
        {
            ViewBag.User_Id = id;
            return View();
        }

        [Route("Library/Create/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Library library, HttpPostedFileBase file, Guid id)
        {
            
            string fileName = Path.GetFileName(file.FileName);
            if (fileName != null)
            {
                var path = Path.Combine(Server.MapPath("~/Image"), library.Name+".jpg");

                file.SaveAs(path);
            }
            library.Address = new Address()
            {
                City = library.Address.City,
                Contry = library.Address.Contry,
                PostCode = library.Address.PostCode,
                Street = library.Address.Street,
                Number = library.Address.Number
            };
            library.Photo = library.Name + ".jpg";
            try
            {
                if (_libraryRepository.AddLibrary(library,_libraryRepository.GetUserByGuid(id) ))
                {
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