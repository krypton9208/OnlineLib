﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using OnlineLib.Models;
using OnlineLib.Repository.IRepository;
using OnlineLib.Repository.ViewModels;

namespace OnlineLib.Repository.Repository
{
    public class BooksRepository : IBooksRepository
    {
        private readonly OnlineLibDbContext _db;

        public BooksRepository(OnlineLibDbContext db)
        {
            _db = db;
        }

        public ICollection<Book> GetBooks(int lib)
        {
            return _db.Library.First(x => x.Id == lib).Books;
        }

        public Book GetBookById(int id)
        {
            return _db.Book.First(x => x.Id == id);
        }

        public bool Add(Book book, int lib)
        {
            if (book != null && lib != 0)
            {
                book.Library = _db.Library.First(x => x.Id == lib);
                _db.Book.Add(book);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool Update(Book book)
        {
            if (book != null)
            {
                var dd = _db.Book.First(x => x.Id == book.Id);
                dd.Autor = book.Autor;
                dd.Title = book.Title;
                dd.Isbn = book.Isbn;
                dd.Lended = book.Lended;
                dd.LoadActivity = book.LoadActivity;
                dd.ShortId = book.ShortId;
                _db.Entry(dd).State = EntityState.Modified;
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool Remove(Book book, int lib)
        {
            if (!book.Lended)
            {
                var d = _db.Book.First(x => x.Id == book.Id);
                _db.Library.First(x => x.Id == lib).Books.Remove(d);
                _db.Book.Remove(d);
                try
                {
                    _db.SaveChanges();

                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool SaveChanges()
        {
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool IsBooked(int id)
        {
            return _db.Book.First(x => x.Id == id).Lended;
        }

        public bool IsPrinted(Book book)
        {
            return _db.Book.Find(book).ShortId.Length > 0;
        }

        public List<BookToPrint> BookToPrint(int lib)
        {
            var list = new List<BookToPrint>();
            foreach (Book item in _db.Book.Where(x=>x.LibraryId == lib && x.ShortId.Length <=0))
            {
                list.Add(new BookToPrint
                {
                    Book =  item,
                    Print = false
                });
            }
            return list;
        }



        public List<BookLabel> GenerateCodeToPrint(List<int> lista)
        {
            List<BookLabel> result = new List<BookLabel>();
            foreach (var book in lista)
            {
                if (book != 0)
                {
                    var temp = _db.Book.First(x => x.Id == book);

                    var label = new BookLabel(temp.Library.Name, GetUniqueKey(), temp.Title);
                    _db.Book.First(x => x.Id == book).ShortId = label.GetCode;
                    
                    result.Add(new BookLabel(temp.Library.Name, GetUniqueKey(), temp.Title));
                }
            }
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }
            return result;
        }


        public BookLabel GenerateCodeToPrint(int book)
        {
            if (book != 0)
            {
                var temp = _db.Book.First(x => x.Id == book);

                var label = new BookLabel(temp.Library.Name, temp.Id.ToString("D13"), temp.Title);
                _db.Book.First(x => x.Id == book).ShortId = label.GetCode;
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    return new BookLabel();
                }
                return label;
            }
            return new BookLabel();
        }

        private string GetUniqueKey()
        {
            int maxSize = 13;
            int minSize = 13;
            // ReSharper disable once RedundantAssignment
            char[] chars = new char[70];
            const string a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            chars = a.ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            int size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - minSize)]); }
            return result.ToString();
        }

        public class BookLabel
        {
           private string LibraryName { get; }
           private string Code { get;  }
           private string BookName { get;  }

            public BookLabel() { }

            public BookLabel(string libname, string code, string bookname)
            {
                LibraryName = libname;
                Code = code;
                BookName = bookname;
            }

            public string GetLibraryName => LibraryName;
            public string GetCode => Code;
            public string GetBookName => BookName;
        }
    }
}
