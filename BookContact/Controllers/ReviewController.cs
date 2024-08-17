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
    public class ReviewController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ReviewController()
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

        // GET: Review/List
        public ActionResult List()
        {
            //curl "https://localhost:44324/api/reviewdata/listreviews"
            string url = "reviewdata/listreviews";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ReviewDto> reviews = response.Content.ReadAsAsync<IEnumerable<ReviewDto>>().Result;

            return View(reviews); ;
        }

        // GET: Review/Details/5
        public ActionResult Details(int id)
        {
            string url = "reviewdata/findreview/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ReviewDto selectedReview = response.Content.ReadAsAsync<ReviewDto>().Result;

            return View(selectedReview);
        }

         public ActionResult Error()
        {
            return View();
        }

        // GET: Review/New
        public ActionResult New()
        {
            string url = "bookdata/listbooks";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<BookDto> BookOptions = response.Content.ReadAsAsync<IEnumerable<BookDto>>().Result;

            return View(BookOptions);
        }

        // POST: Review/Create
        [HttpPost]
        public ActionResult Create(Review review)
        {
            string url = "reviewdata/addreview";
            string jsonpayload = jss.Serialize(review);
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


        // GET: Review/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateReview ViewModel = new UpdateReview();

            string url = "reviewdata/findreview/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ReviewDto selectedReview = response.Content.ReadAsAsync<ReviewDto>().Result;
            ViewModel.SelectedReview = selectedReview;

            url = "bookdata/listbooks/";
            response = client.GetAsync(url).Result;
            IEnumerable<BookDto> BookOptions = response.Content.ReadAsAsync<IEnumerable<BookDto>>().Result;
            ViewModel.BookOptions = BookOptions;

            return View(ViewModel);
        }

        // POST: Review/Update/5
        [HttpPost]
        public ActionResult Update(int id, Review review)
        {
            string url = "reviewdata/updatereview/" + id;
            string jsonpayload = jss.Serialize(review);
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

        // GET: Review/Delete/5
        public ActionResult ConfirmDelete(int id)
        {
            string url = "reviewdata/findreview/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ReviewDto selectedReview = response.Content.ReadAsAsync<ReviewDto>().Result;

            return View(selectedReview);
        }

        // POST: Review/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "reviewdata/deletereview/" + id;
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
