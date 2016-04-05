using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using OnlineLib.Models;
using OnlineLib.Repository.IRepository;
using OnlineLib.Repository.Repository;
using OnlineLib.Repository.ViewModels;

namespace OnlineLib.App.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBooksRepository _booksRepository;
        private readonly ILibraryRepository _libraryRepository;
        // GET: Books
        public BooksController(IBooksRepository repository, ILibraryRepository repo)
        {
            _booksRepository = repository;
            _libraryRepository = repo;
        }


        [Route("{lib}/Books")]
        public ActionResult Index(int lib)
        {
            ViewBag.Library = lib;
            ViewBag.Name = _libraryRepository.GetLibraryById(lib).Name;
            ViewBag.Worker = _libraryRepository.IsWorker(lib, Guid.Parse(User.Identity.GetUserId()));
            ViewBag.LibOwner = _libraryRepository.IsLibOwner(Guid.Parse(User.Identity.GetUserId()), lib);
            return View(_booksRepository.GetBooks(lib));
        }

        [Route("{lib}/Books/Add")]
        public ActionResult Add(int lib)
        {
            if (_libraryRepository.IsWorker(lib, Guid.Parse(User.Identity.GetUserId())))
            {
                ViewBag.Library = lib;
                return View();
            }
            return RedirectToAction("Index");

        }

        [Route("{lib}/Books/Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Book book, int? lib)
        {

            if (ModelState.IsValid && lib != null && _libraryRepository.IsWorker((int)lib, Guid.Parse(User.Identity.GetUserId())))
            {
                if (_booksRepository.Add(book, (int)lib))
                    return RedirectToAction("Index", new { @lib = lib });
                ViewBag.Library = lib;
                return View(book);

            }
            ViewBag.Library = lib;
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

        [Route("{lib}/Books/PdfGeneratorBook/{id}")]
        public ActionResult PdfGeneratorBook(int id, int lib)
        {
            var item = _booksRepository.GenerateCodeToPrint(id);
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 20, 20, 30, 20);

                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                writer.SetPdfVersion(iTextSharp.text.pdf.PdfWriter.PDF_VERSION_1_7);
                document.Open();
                MultiColumnText columns = new MultiColumnText();
                columns.AddRegularColumns(30f, document.PageSize.Width - 30f, 30f, 3);
                var namefont = FontFactory.GetFont("Times New Roman", 10);
                PdfContentByte cb = writer.DirectContent;

                columns.AddElement(new Chunk("   " + item.GetLibraryName, namefont));
                columns.AddElement(Chunk.NEWLINE);
                columns.AddElement(Barcode(item.GetCode).CreateImageWithBarcode(cb, null, null));
                columns.AddElement(new Chunk(item.GetBookName, namefont));
                columns.AddElement(Chunk.NEWLINE);

                document.Add(columns);
                document.Close();
                writer.Close();
                return File(ms.ToArray(), "application/pdf", _booksRepository.GetBookById(id).Title + ".pdf");
            }

        }


        [Route("{lib}/Books/SelectToPrint")]
        public ActionResult SelectToPrint(int lib)
        {
            ViewBag.Library = lib;
            return View(_booksRepository.BookToPrint(lib));
        }

        [Route("{lib}/Books/SelectToPrint")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SelectToPrint(int lib, List<BookToPrint> model)
        {
            List<int> t = new List<int>();
            foreach (var item in model)
            {
                if (item.Print) t.Add(item.Book.Id);
            }
            if (t.Count > 0)
                return PdfGeneratorBooks(t, lib);
            return RedirectToAction("Index", new { @lib = lib });
        }

        [Route("{lib}/Books/PdfGeneratorBooks/{id}")]
        [HttpPost]
        public ActionResult PdfGeneratorBooks(List<int> id, int lib)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 0, 0, 30, 0);

                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                writer.SetPdfVersion(iTextSharp.text.pdf.PdfWriter.PDF_VERSION_1_7);

                document.Open();
                MultiColumnText columns = new MultiColumnText();
                columns.AddRegularColumns(30f, document.PageSize.Width - 30f, 30f, 3);
                var namefont = FontFactory.GetFont("Times New Roman", 10);
                PdfContentByte cb = writer.DirectContent;
                int i = 1;
                foreach (BooksRepository.BookLabel item in _booksRepository.GenerateCodeToPrint(id))
                {
                    columns.AddElement(new Chunk("   " + item.GetLibraryName, namefont));
                    columns.AddElement(Chunk.NEWLINE);
                    columns.AddElement(Barcode(item.GetCode).CreateImageWithBarcode(cb, null, null));
                    columns.AddElement(new Chunk(item.GetBookName, namefont));
                    columns.AddElement(Chunk.NEWLINE);
                    if (i % 7 == 0)
                    {
                        columns.AddElement(Chunk.NEWLINE);
                        columns.AddElement(Chunk.NEWLINE);
                        columns.AddElement(Chunk.NEWLINE);
                    }
                    i++;
                }
                document.Add(columns);
                document.Close();
                writer.Close();
                return File(ms.ToArray(), "application/pdf", "Lista Etykiet.pdf");
            }

        }

        private static Barcode128 Barcode(string text)
        {
            Barcode128 code = new Barcode128
            {
                ChecksumText = true,
                AltText = text,
                GenerateChecksum = true,
                StartStopText = true,
                Code = text,
                Extended = true,
                Size = 8,
                TextAlignment = Element.ALIGN_MIDDLE
            };
            return code;
        }

    }


}