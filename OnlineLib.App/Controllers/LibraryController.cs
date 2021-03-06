﻿using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineLib.Models;
using OnlineLib.Repository.IRepository;
using Microsoft.AspNet.Identity;


namespace OnlineLib.App.Controllers
{
    [Authorize]
    public class LibraryController : Controller
    {
        private readonly ILibraryRepository _libraryRepository;


        public LibraryController(ILibraryRepository _repo)
        {
            _libraryRepository = _repo;
        }

        

        [Route("{lib}/Library/Home")]
        public ActionResult Index(int lib)
        {
            ViewBag.Library = lib;

            ViewBag.Sub = _libraryRepository.UserSubscibeLibrary(lib, Guid.Parse(User.Identity.GetUserId()));

            return View(_libraryRepository.GetLibraryById(lib));
        }
        [Authorize]
        [Route("{lib}/Library/SubscribeLibrary")]
        public ActionResult SubscribeLibrary(int lib)
        {
            Guid userid = Guid.Parse(User.Identity.GetUserId());
            if (lib != 0 && userid != Guid.Empty)
            {
                if (_libraryRepository.Subscribe(lib, userid))
                    return RedirectToAction("Index", "Books", new { @lib = lib });
            }
            return RedirectToAction("Index", "Library", new { @lib = lib });
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
        [Authorize]
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

        [Route("Library/Search")]
        public ActionResult Search(string search)
        {
            if (string.IsNullOrEmpty(search))
                return View(_libraryRepository.GetLibraryList);
            return
                View(
                    _libraryRepository.GetLibraryList.Where(
                        x =>
                            x.Name.Contains(search) || x.Address.City.Contains(search) ||
                            x.Address.Street.Contains(search)));
        }


    }
}