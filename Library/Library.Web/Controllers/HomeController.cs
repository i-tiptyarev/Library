using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Library.Core.Models;

namespace Library.Web.Controllers
{
    public class HomeController : Controller
    {
        Models.LibraryContext db = new Models.LibraryContext();

        [HttpGet]
        public ActionResult Books()
        {
            int UserId = (int)Session["UserId"];

            var Reader = db.Readers.Where(x => x.Id == UserId).FirstOrDefault();

            if (Reader is Readers)
            {
                ViewBag.Title = (Reader as Readers).FIO; //получение имени читателя

                var BooksIds = db.ReadersBooks.Where(x => x.ReaderId == UserId).Select(x => x.BookId).ToList();

                var Books = db.Books.Where(x => BooksIds.Contains(x.Id)).ToList();

                return View(Books);
            }
            else
            {
                ViewBag.Title = "Error";

                return View();
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Users user)
        {
            var User = db.Users.Where(x => x.Login == user.Login).FirstOrDefault();

            if (User != null && User.Password == user.Password)
            {
                Session["UserId"] = User.Id;

                if (!User.isAdmin)
                {
                    return RedirectToAction("Books");
                }
                else return RedirectToAction("AdminPanel");
            }
            else
            {
                return View();
            }

        }

        public ActionResult AdminPanel()
        {
            return View();
        }

        public ActionResult AddBookForm()
        {
            var books = db.Books.Where(x => x.GiveBackDate == null).ToList();
            return View(books);
        }

        public ActionResult AddBook(Books book)
        {
            ReadersBooks temp = new ReadersBooks { BookId = book.Id, ReaderId = (int)Session["Id"] };

            db.Entry(book).State = System.Data.Entity.EntityState.Modified;
            db.Entry(temp).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("AdminReaders");
        }

        public ActionResult DeleteBook(int id)
        {
            ReadersBooks rb = db.ReadersBooks.Find(id);
            Books book = db.Books.Find(id);
            book.GiveBackDate = null;
            var readerId = db.ReadersBooks.Find(id).ReaderId;

            db.Entry(rb).State = System.Data.Entity.EntityState.Deleted;
            db.Entry(book).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("AdminReaders");
        }

        [HttpGet]
        public ActionResult ReserveBook()
        {
            int UserId = (int)Session["UserId"];

            var Reader = db.Readers.Where(x => x.Id == UserId).FirstOrDefault();

            if (Reader is Readers)
            {
                ViewBag.Title = (Reader as Readers).FIO; //получение имени читателя

                var BooksIds = db.ReadersBooks.Where(x => x.ReaderId != UserId).Select(x => x.BookId).ToList();

                var Books = db.Books.Where(x => BooksIds.Contains(x.Id)).ToList();

                return View(Books);
            }
            else
            {
                ViewBag.Title = "Error";

                return View();
            }
        } //Не дописано

        [HttpPost]
        public ActionResult ReserveBook(Books book) // Не дописано
        {

            return View();
        }

        public ActionResult AdminBooks()
        {
            return View(db.Books);
        }

        [HttpGet]
        public ActionResult AdminAddBook()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminAddBook(Books book)
        {

            db.Entry(book).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            return RedirectToAction("AdminBooks");
        }

        [HttpGet]
        public ActionResult AdminEditBook(int id)
        {
            Books book = db.Books.Find(id);
            return View(book);
        }

        [HttpPost]
        public ActionResult AdminEditBook(Books book)
        {
            db.Entry(book).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("AdminBooks");
        }

        public ActionResult AdminDeleteBook(int id)
        {
            Books book = db.Books.Find(id);
            db.Entry(book).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("AdminBooks");
        }

        public ActionResult AdminReaders()
        {
            return View(db.Readers);
        }

        [HttpGet]
        public ActionResult AdminAddReader()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminAddReader(Readers reader)
        {
            string password = null;

            Random rand = new Random();
            for(int i = 0; i < 8; i++)
            {
                char c = (char)rand.Next(97, 122);
                if (rand.Next(0, 10) % 2 == 0) c = Char.ToUpper(c);
                else c = Char.ToLower(c);

                password += c;
            }

            db.Entry(reader).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            
            Users user = new Users { Login = "User" + reader.Id, Password = password };
            db.Entry(user).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            return RedirectToAction("AdminReaders");
        }

        [HttpGet]
        public ActionResult AdminEditReader(int id)
        {
            Readers reader = db.Readers.Find(id);
            return View(reader);
        }

        [HttpPost]
        public ActionResult AdminEditReader(Readers reader)
        {
            db.Entry(reader).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AdminReaders");
        }

        public ActionResult AdminDeleteReader(int id)
        {
            Readers reader = db.Readers.Find(id);
            db.Entry(reader).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("AdminReaders");
        }

        public ActionResult AdminDetailsReader(int id)
        {
            ViewBag.ReaderName = db.Readers.Find(id).FIO;
            ViewBag.Login = db.Users.Find(id).Login;
            ViewBag.Password = db.Users.Find(id).Password;
            Session["Id"] = id;

            var BooksIds = db.ReadersBooks.Where(x => x.ReaderId == id).Select(x => x.BookId).ToList();

            var Books = db.Books.Where(x => BooksIds.Contains(x.Id)).ToList();

            return View(Books);
        }

        public ActionResult IndexTest()
        {
            return View();
        }
    }
}
