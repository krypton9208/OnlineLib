using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineLib.Models;
using OnlineLib.Repository.IRepository;

namespace OnlineLib.App.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBooksRepository _booksRepository;
        private readonly ILibraryRepository _libraryRepository;
        // GET: Books
        public BooksController(IBooksRepository _repository, ILibraryRepository _repo)
        {
            _booksRepository = _repository;
            _libraryRepository = _repo;
        }

        [Route("{lib}/Books")]
        public ActionResult Index(int lib)
        {
            ViewBag.Library = lib;
            return View(_booksRepository.GetBooks(lib));
        }

        [Route("{lib}/Books/Add")]
        public ActionResult Add(int lib)
        {
            ViewBag.Library = lib;
            return View();
        }

        [Route("{lib}/Books/Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Book book, int? lib)
        {
            
            if (ModelState.IsValid && lib != null)
            {
                _booksRepository.Add(book,(int) lib);
             return RedirectToAction("Index", new {@lib = lib});
            }
            return View(book);
        }

        [Route("{lib}/Books/Edit/{id}")]
        public ActionResult Edit(int lib, int id)
        {
            ViewBag.Library = lib;
            ViewBag.Id = id;
            return View(_booksRepository.GetBookById(id));
        }

        [Route("{lib}/Books/Edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book, int lib)
        {
            if (ModelState.IsValid)
            {
                if (_booksRepository.Update(book))
                {
                    return RedirectToAction("Index", new {@lib = lib});
                }
                ViewBag.Library = lib;
                ViewBag.Id = book.Id;
                return View(book);
            }
            ViewBag.Library = lib;
            ViewBag.Id = book.Id;
            return View(book);
        }

        [Route("{lib}/Books/Delete/{id}")]
        public ActionResult Delete(int lib, int id)
        {
            ViewBag.Library = lib;
            ViewBag.Id = id;
            return View(_booksRepository.GetBookById(id));
        }

        [Route("{lib}/Books/Delete/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Book book, int lib)
        {
            if (ModelState.IsValid)
            {
                if (_booksRepository.Remove(book, lib))
                {
                    return RedirectToAction("Index", new { @lib = lib });
                }
                ViewBag.Library = lib;
                ViewBag.Id = book.Id;
                return View(book);
            }
            ViewBag.Library = lib;
            ViewBag.Id = book.Id;
            return View(book);
        }
    }
}