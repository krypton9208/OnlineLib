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
        [Route("Library/Create")]
        public ActionResult Create()
        {
            return View();
        }

        [Route("Library/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Library library, HttpPostedFileBase file)
        {
            
            string fileName = Path.GetFileName(file.FileName);
            if (fileName != null)
            {
                var path = Path.Combine(Server.MapPath("~/Image"), fileName);

                file.SaveAs(path);
            }
            library.Photo = fileName;
            try
            {
                if (_libraryRepository.AddLibrary(library))
                {
                    int idlib = _libraryRepository.GetLibraryByName(library.Name).Id;
                    return RedirectToActionPermanent("Register", "Account", new {lib = idlib, owner = true, worker = false});
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