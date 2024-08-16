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
    public class BookController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BookController()
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

        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Book/List
        public ActionResult List()
        {
            //curl "https://localhost:44324/api/bookdata/listbooks"
            string url = "bookdata/listbooks";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<BookDto> books = response.Content.ReadAsAsync<IEnumerable<BookDto>>().Result;

            return View(books);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            DetailsBook ViewModel = new DetailsBook();

            string url = "bookdata/findbook/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            BookDto selectedBook = response.Content.ReadAsAsync<BookDto>().Result;
            Debug.WriteLine("Book received:");
            Debug.WriteLine(selectedBook.Title);

            ViewModel.SelectedBook = selectedBook;

            url = "rentaldata/listbooksforrental/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<RentalDto> RentedBooks = response.Content.ReadAsAsync<IEnumerable<RentalDto>>().Result;

            ViewModel.RentedBooks = RentedBooks;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Book/New
        public ActionResult New()
        {
            string url = "authordata/listauthors";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<AuthorDto> AuthorOptions = response.Content.ReadAsAsync<IEnumerable<AuthorDto>>().Result;

            return View(AuthorOptions);
        }

        // POST: Book/Create
        [HttpPost]
        public ActionResult Create(Book book)
        {
            string url = "bookdata/addbook";
            string jsonpayload = jss.Serialize(book);
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

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateBook ViewModel = new UpdateBook();

            string url = "bookdata/findbook/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BookDto selectedBook = response.Content.ReadAsAsync<BookDto>().Result;
            ViewModel.SelectedBook = selectedBook;

            url = "authordata/listauthors/";
            response = client.GetAsync(url).Result;
            IEnumerable<AuthorDto> AuthorOptions = response.Content.ReadAsAsync<IEnumerable<AuthorDto>>().Result;
            ViewModel.AuthorOptions = AuthorOptions;

            return View(ViewModel);
        }

        // POST: Book/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Book book)
        {
            GetApplicationCookie();
            string url = "bookdata/updatebook/" + id;
            string jsonpayload = jss.Serialize(book);
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

        // GET: Book/Delete/5
        public ActionResult ConfirmDelete(int id)
        {
            string url = "bookdata/findbook/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BookDto selectedBook = response.Content.ReadAsAsync<BookDto>().Result;

            return View(selectedBook);
        }

        // POST: Book/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "bookdata/deletebook/" + id;
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
