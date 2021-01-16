/// @Clara
/// @ Gabriel

/// Controller for communication between database and frontend. Handles the user input from a search and tells the database to perform it. 


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRINCESS.model;

namespace PRINCESS.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        PrincessDb pDb = new PrincessDb();
        List<Review> reviews;
        List<ReviewFormat> rev = new List<ReviewFormat>();

        [Route("api/[controller]/Login")]
        [HttpGet]
        public Boolean Connect(string email, string password)
        {
            return true;
        }

        //string reviews;

        // GET: api/Database
        [HttpGet]
        public void Get()
        {

        }

        // GET: api/Database/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }


        public class SearchKeys
        {
             public string SearchString { get; set; }
             public string SearchType { get; set; }
             
        }


        // POST: api/Database
        [HttpPost]
        public void Post(SearchKeys searchKey)
        {

            switch (searchKey.SearchType)
            {
                case "Title":
                    reviews = new List<Review>(PrincessDb.searchReviewByTitle(searchKey.SearchString));
                    break;
                case "Domain":
                    reviews = new List<Review>(PrincessDb.searchReviewByDomain(searchKey.SearchString));
                    break;
                case "Writer":
                    reviews = new List<Review>(PrincessDb.searchReviewByWriter(searchKey.SearchString));
                    break;
                default:
                    reviews = new List<Review>(PrincessDb.searchReviewByTitle(searchKey.SearchString));
                    break;
            }

            for (int i = 0; i < reviews.Count(); i++)
            {
                rev.Add(new ReviewFormat
                {
                    Url = reviews.ElementAt(i).getUrl(),
                    Title = reviews.ElementAt(i).getTitle(),
                    Rating = reviews.ElementAt(i).getRating(),
                    ReviewText = reviews.ElementAt(i).getReviewText(),
                    Writer = reviews.ElementAt(i).getWriter(),
                    DomainUrl = reviews.ElementAt(i).getDomainUrl(),
                });
            }

            string JSONresult = JsonConvert.SerializeObject(rev);
            string path = @"C:\PRINCESS\PRINCESS\princess\src\components\JsonData\reviews.json";

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(JSONresult.ToString());
                    tw.Close();
                }
            }
            else if (!System.IO.File.Exists(path))
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(JSONresult.ToString());
                    tw.Close();
                }
            }

            Console.WriteLine(reviews);
            //PrincessDb.disconnect();

        }

        // PUT: api/Database/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}