using BookContact.Models;
using BookContact.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BookContact.Controllers
{
    public class ContactController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ContactController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44324/api/");
        }

        // GET: Contact/List
        public ActionResult List()
        {
            //curl "https://localhost:44324/api/contactdata/listcontacts"
            string url = "contactdata/listcontacts";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ContactDto> contacts = response.Content.ReadAsAsync<IEnumerable<ContactDto>>().Result;
            return View(contacts);
        }

        // GET: Contact/Details/5
        public ActionResult Details(int id)
        {
            string url = "contactdata/findcontact/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ContactDto selectedContact = response.Content.ReadAsAsync<ContactDto>().Result;

            return View(selectedContact);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Contact/Create
        public ActionResult New()
        {
            string url = "authordata/listauthors";
            string urlRental = "rentaldata/listrentals";
            HttpResponseMessage response = client.GetAsync(url).Result;
            HttpResponseMessage rentalResponse = client.GetAsync(urlRental).Result;
            IEnumerable<AuthorDto> AuthorOptions = response.Content.ReadAsAsync<IEnumerable<AuthorDto>>().Result;
            IEnumerable<RentalDto> RentalOptions = rentalResponse.Content.ReadAsAsync<IEnumerable<RentalDto>>().Result;

            var contactDto = new ContactDto();
            var viewModel = new DetailsContact
            {
                Contact = contactDto,
                AuthorOption = AuthorOptions,
                RentalOption = RentalOptions
            };

            return View(viewModel);
        }

        // POST: Contact/Create
        [HttpPost]
        public ActionResult Create(Contact contact)
        {
            string url = "contactdata/addcontact";
            string jsonpayload = jss.Serialize(contact);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Contact/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Contact/Update/5
        [HttpPost]
        public ActionResult Update(int id, Contact contact)
        {
            string url = "contactdata/updatecontact/" + id;
            string jsonpayload = jss.Serialize(contact);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                var errorMessage = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"Update request failed with status code: {response.StatusCode}");
                Debug.WriteLine($"Error message: {errorMessage}");
                return RedirectToAction("Error");
            }
        }

        // GET: Contact/Delete/5
        public ActionResult ConfirmDelete(int id)
        {
            string url = "contactdata/findcontact/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ContactDto selectedContact = response.Content.ReadAsAsync<ContactDto>().Result;

            return View(selectedContact);
        }

        // POST: Contact/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "contactdata/deletecontact/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                var errorMessage = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"DELETE request failed with status code: {response.StatusCode}");
                Debug.WriteLine($"Error message: {errorMessage}");
                return RedirectToAction("Error");
            }
        }
    }
}
