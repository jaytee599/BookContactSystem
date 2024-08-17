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

namespace BookContact.Controllers
{
    public class ContactDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ContactData/listContacts
        [HttpGet]
        [ResponseType(typeof(ContactDto))]
        public IHttpActionResult ListContacts()
        {
            List<Contact> Contacts = db.Contacts.ToList();
            List<ContactDto> ContactDtos = new List<ContactDto>();

            Contacts.ForEach(c => ContactDtos.Add(new ContactDto()
            {
                ContactId = c.ContactId,
                Name = c.Name,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Address = c.Address,
                AuthorName = c.Author.AuthorName
            }));

            return Ok(ContactDtos);
        }

        // GET: api/ContactData/findContact/5
        [ResponseType(typeof(Contact))]
        [HttpGet]
        public IHttpActionResult FindContact(int id)
        {
            Contact contact = db.Contacts.Find(id);
            ContactDto contactDto = new ContactDto()
            {
                ContactId = contact.ContactId,
                Name = contact.Name,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,
                Address = contact.Address,
                AuthorName = contact.Author.AuthorName
            };
            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contactDto);
        }

        // PUT: api/ContactData/updateContact/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateContact(int id, Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contact.ContactId)
            {
                return BadRequest();
            }

            db.Entry(contact).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
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

        // POST: api/ContactData/addContact
        [ResponseType(typeof(Contact))]
        [HttpPost]
        public IHttpActionResult AddContact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Contacts.Add(contact);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = contact.ContactId }, contact);
        }

        // DELETE: api/ContactData/deleteContact/5
        [ResponseType(typeof(Contact))]
        [HttpPost]
        public IHttpActionResult DeleteContact(int id)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }

            db.Contacts.Remove(contact);
            db.SaveChanges();

            return Ok(contact);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContactExists(int id)
        {
            return db.Contacts.Count(e => e.ContactId == id) > 0;
        }
    }
}