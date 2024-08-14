using BookContact.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using BookContact.Models.ViewModels;
using System.Web.Script.Serialization;

namespace BookContact.Controllers
{
    public class AuthorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AuthorController()
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

        // GET: Author/List
        public ActionResult List()
        {
            //curl "https://localhost:44324/api/authordata/listauthors"
            string url = "authordata/listauthors";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<AuthorDto> authors = response.Content.ReadAsAsync<IEnumerable<AuthorDto>>().Result;

            return View(authors);
        }

        // GET: Author/Details/5
        public ActionResult Details(int id)
        {
            DetailsAuthor ViewModel = new DetailsAuthor();

            string url = "authordata/findauthor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AuthorDto selectedAuthor = response.Content.ReadAsAsync<AuthorDto>().Result;

            ViewModel.SelectedAuthor = selectedAuthor;

            //showcase info about books related to this author
            //send a request to gather info about books related to a particular author ID
            url = "bookdata/listbooksforauthors/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<BookDto> RelatedBooks = response.Content.ReadAsAsync<IEnumerable<BookDto>>().Result;
            ViewModel.RelatedBooks = RelatedBooks;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Author/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Author/Create
        [HttpPost]
        public ActionResult Create(Author author)
        {
            string url = "authordata/addauthor";
            string jsonpayload = jss.Serialize(author);
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

        // GET: Author/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "authordata/findauthor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AuthorDto selectedAuthor = response.Content.ReadAsAsync<AuthorDto>().Result;

            return View(selectedAuthor);
        }

        // POST: Author/Update/5
        [HttpPost]
        public ActionResult Update(int id, Author author)
        {
            string url = "authordata/updateauthor/" + id;
            string jsonpayload = jss.Serialize(author);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Author/Delete/5
        public ActionResult ConfirmDelete(int id)
        {
            string url = "authordata/findauthor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AuthorDto selectedAuthor = response.Content.ReadAsAsync<AuthorDto>().Result;

            return View(selectedAuthor);
        }

        // POST: Author/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "authordata/deleteauthor/" + id;
            HttpContent content = new StringContent("");
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
    }
}
