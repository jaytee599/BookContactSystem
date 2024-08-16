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
    public class ReviewDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ReviewData/ListReviews
        [HttpGet]
        [ResponseType(typeof(ReviewDto))]
        public IHttpActionResult ListReviews()
        {
            List<Review> Reviews = db.Reviews.ToList();
            List<ReviewDto> ReviewDtos = new List<ReviewDto>();
            Reviews.ForEach(a => ReviewDtos.Add(new ReviewDto()
            {
                ReviewId = a.ReviewId,
                Reviews = a.Reviews,
                Comments = a.Comments,
                BookId = a.BookId,
                Title = a.Book.Title,
            }));
            return Ok(ReviewDtos);
        }

        // GET: api/ReviewData/ListReviewsForBook/3
        [HttpGet]
        [ResponseType(typeof(ReviewDto))]
        public IHttpActionResult ListReviewsForBook(int id)
        {
            List<Review> Reviews = db.Reviews.Where(a => a.BookId == id).ToList();
            List<ReviewDto> ReviewDtos = new List<ReviewDto>();
            Reviews.ForEach(a => ReviewDtos.Add(new ReviewDto()
            {
                ReviewId = a.ReviewId,
                Reviews = a.Reviews,
                Comments = a.Comments,
                BookId = a.BookId,
                Title = a.Book.Title,
            }));
            return Ok(ReviewDtos);
        }

        // GET: api/ReviewData/FindReview/5
        [ResponseType(typeof(ReviewDto))]
        [HttpGet]
        public IHttpActionResult FindReview(int id)
        {
            Review review = db.Reviews.Find(id);
            ReviewDto reviewDto = new ReviewDto()
            {
                ReviewId = review.ReviewId,
                Reviews = review.Reviews,
                Comments = review.Comments,
                BookId = review.BookId,
                Title = review.Book.Title,
            };
            if (review == null)
            {
                return NotFound();
            }

            return Ok(reviewDto);
        }

        // PUT: api/ReviewData/UpdateReview/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateReview(int id, Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != review.ReviewId)
            {
                return BadRequest();
            }

            db.Entry(review).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        // POST: api/ReviewData/AddReview
        [ResponseType(typeof(Review))]
        [HttpPost]
        public IHttpActionResult AddReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reviews.Add(review);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = review.ReviewId }, review);
        }

        // DELETE: api/ReviewData/DeleteReview/5
        [ResponseType(typeof(Review))]
        [HttpPost]
        public IHttpActionResult DeleteReview(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }

            db.Reviews.Remove(review);
            db.SaveChanges();

            return Ok(review);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReviewExists(int id)
        {
            return db.Reviews.Count(e => e.ReviewId == id) > 0;
        }
    }
}