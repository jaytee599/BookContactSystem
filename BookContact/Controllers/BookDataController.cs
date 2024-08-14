using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BookContact.Models;

namespace BookContact.Controllers
{
    public class BookDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/BookData/ListBooks
        [HttpGet]
        [ResponseType(typeof(BookDto))]
        public IHttpActionResult ListBooks()
        {
            List<Book> Books = db.Books.ToList();
            List<BookDto> BookDtos = new List<BookDto>();
            Books.ForEach(a => BookDtos.Add(new BookDto()
            {
                BookId = a.BookId,
                Title = a.Title,
                ISBN = a.ISBN,
                Price = a.Price,
                BorrowingDate = a.BorrowingDate,
                AuthorName = a.Author.AuthorName,
            }));
            return Ok(BookDtos);
        }

        // GET: api/BookData/ListBooksForAuthors
        [HttpGet]
        [ResponseType(typeof(BookDto))]
        public IHttpActionResult ListBooksForAuthors(int id)
        {
            List<Book> Books = db.Books.Where(a => a.AuthorId == id).ToList();
            List<BookDto> BookDtos = new List<BookDto>();
            Books.ForEach(a => BookDtos.Add(new BookDto()
            {
                BookId = a.BookId,
                Title = a.Title,
                ISBN = a.ISBN,
                Price = a.Price,
                BorrowingDate = a.BorrowingDate,
                AuthorName = a.Author.AuthorName,
            }));
            return Ok(BookDtos);
        }

        // GET: api/BookData/FindBook/5
        [ResponseType(typeof(BookDto))]
        [HttpGet]
        public IHttpActionResult FindBook(int id)
        {
            Book book = db.Books.Find(id);
            BookDto BookDto = new BookDto()
            {
                BookId = book.BookId,
                Title = book.Title,
                ISBN = book.ISBN,
                Price = book.Price,
                BorrowingDate = book.BorrowingDate,
                AuthorName = book.Author.AuthorName,
            };
            if (book == null)
            {
                return NotFound();
            }

            return Ok(BookDto);
        }

        // POST: api/BookData/UpdateBook/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateBook(int id, Book book)
        {
            Debug.WriteLine("Update Book Successful");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.BookId)
            {
                return BadRequest();
            }

            db.Entry(book).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {

                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BookData/AddBook
        [ResponseType(typeof(Book))]
        public IHttpActionResult AddBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Books.Add(book);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = book.BookId }, book);
        }

        // POST: api/BookData/DeleteBook/5
        [ResponseType(typeof(Book))]
        [HttpPost]
        public IHttpActionResult DeleteBook(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            db.Books.Remove(book);
            db.SaveChanges();

            return Ok(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return db.Books.Count(e => e.BookId == id) > 0;
        }
    }
}