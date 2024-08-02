using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BookContact.Models;
using System.Diagnostics;

namespace BookContact.Controllers
{
    public class AuthorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AuthorData/ListAuthors
        [HttpGet]
        [ResponseType(typeof(AuthorDto))]
        public IHttpActionResult ListAuthors()
        {
            List<Author> Authors = db.Authors.ToList();
            List<AuthorDto> AuthorDtos = new List<AuthorDto>();

            Authors.ForEach(a => AuthorDtos.Add(new AuthorDto()
            {
                AuthorId = a.AuthorId,
                AuthorName = a.AuthorName,
                Biography = a.Biography,
            }));
            return Ok(AuthorDtos);
        }

        // GET: api/AuthorData/FindAuthor/5
        [ResponseType(typeof(AuthorDto))]
        [HttpGet]
        public IHttpActionResult FindAuthor(int id)
        {
            Author Author = db.Authors.Find(id);
            AuthorDto AuthorDto = new AuthorDto()
            {
                AuthorId = Author.AuthorId,
                AuthorName = Author.AuthorName,
                Biography = Author.Biography,
            };
            if (Author == null)
            {
                return NotFound();
            }

            return Ok(AuthorDto);
        }

        // POST: api/AuthorData/UpdateAuthor/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAuthor(int id, Author author)
        {
            Debug.WriteLine("I have reached the updated author");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("invalid model state");
                return BadRequest(ModelState);
            }

            if (id != author.AuthorId)
            {
                Debug.WriteLine("wrong id");
                return BadRequest();
            }

            db.Entry(author).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
                {
                    Debug.WriteLine("Author not available");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Debug.WriteLine("Error no sabi");
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AuthorData/AddAuthor
        [ResponseType(typeof(Author))]
        [HttpPost]
        public IHttpActionResult AddAuthor(Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Authors.Add(author);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = author.AuthorId }, author);
        }

        // POST: api/AuthorData/DeleteAuthor/5
        [ResponseType(typeof(Author))]
        [HttpPost]
        public IHttpActionResult DeleteAuthor(int id)
        {
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return NotFound();
            }

            db.Authors.Remove(author);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AuthorExists(int id)
        {
            return db.Authors.Count(e => e.AuthorId == id) > 0;
        }
    }
}