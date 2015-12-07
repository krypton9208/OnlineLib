using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OnlineLib.Models;
using OnlineLib.Repository.IRepository;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using Image = iTextSharp.text.Image;

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
        public async Task<ActionResult> Add(Book book, int? lib, bool print)
        {

            if (ModelState.IsValid && lib != null)
            {
                _booksRepository.Add(book, (int)lib);
                if (print) await PdfGeneratorBook(book.Id, (int)lib);
                return RedirectToAction("Index", new { @lib = lib });
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
        public async Task<ActionResult> PdfGeneratorBook(int id, int lib)
        {
            string items = id.ToString().PadLeft(17 - id.ToString().Length, ' ');

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 0, 0, 30, 0);

                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                writer.SetPdfVersion(iTextSharp.text.pdf.PdfWriter.PDF_VERSION_1_7);

                document.Open();

                MultiColumnText columns = new MultiColumnText();
                columns.AddRegularColumns(36f, document.PageSize.Width - 36f, 24f, 2);
                columns.AddElement(GetValue(_libraryRepository.GetLibraryById(lib).Name, Barcode(items)));

                document.Add(columns);
                document.Close();
                writer.Close();
                return File(ms.ToArray(), "application/pdf", _booksRepository.GetBookById(id).Title+".pdf");
            }

        }

        private static Image GetValue(string nazwa, Barcode128 code)
        {
            System.Drawing.Bitmap bmpimg = new Bitmap(230, 70);

            Graphics bmpgraphics = Graphics.FromImage(bmpimg);
            bmpgraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            bmpgraphics.CompositingQuality = CompositingQuality.HighQuality;
            bmpgraphics.Clear(Color.White);
            bmpgraphics.SmoothingMode = SmoothingMode.AntiAlias;
            bmpgraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            bmpgraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            bmpgraphics.DrawLine(new Pen(Brushes.Black, 2), new Point(0, 0), new Point(0, bmpimg.Height - 10));
            bmpgraphics.DrawLine(new Pen(Brushes.Black, 2), new Point(0, 0), new Point(bmpimg.Width, 0));
            bmpgraphics.DrawLine(new Pen(Brushes.Black, 2), new Point(bmpimg.Width, 0),
                new Point(bmpimg.Width, bmpimg.Height - 10));
            bmpgraphics.DrawLine(new Pen(Brushes.Black, 2), new Point(0, bmpimg.Height - 10),
                new Point(bmpimg.Width, bmpimg.Height - 10));
            bmpgraphics.DrawString(nazwa, new Font("Times New Roman", 8, FontStyle.Regular), new SolidBrush(Color.Black), new Point(10,4)) ;
            bmpgraphics.DrawImage(code.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White),
                new Point(10, 20));

            bmpgraphics.DrawString(code.Code, new System.Drawing.Font("Times New Roman", 8, FontStyle.Regular), new SolidBrush(Color.Black), new Point(50, 45));
            iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(bmpimg, System.Drawing.Imaging.ImageFormat.Png);
            pdfImage.SetDpi(1200, 1200);

            return pdfImage;
        }

        private static Barcode128 Barcode(string text)
        {
            Barcode128 code128 = new Barcode128();
            code128.CodeType = Barcode128.CODE_AC_TO_B;
            code128.ChecksumText = true;
            code128.AltText = text;
            code128.GenerateChecksum = true;
            code128.StartStopText = true;
            code128.Code = text;
            code128.Extended = false;
            code128.Size = 32;
            code128.TextAlignment = Element.ALIGN_CENTER;
            code128.N = (float) 32;
            code128.X = (float) 32;
            return code128;
        }

    }


}