using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRINCESS.model;

/**
 *  authors:
 *          @Clara Hällgren
 *          @Gabriel Vega
 *          @Anna Mann
 *
 * A controller for communication between database and frontend.
 * Handles the user input from a search and tells the database to perform it.
 * 
 * public void Post(SearchKeys searchKey)
 * Fetches reviews from the database depending on searchKey, the rewievs gets written on a Json-file.
 * parameters:
 *      searchKey - A structure type that encapsulates the search string.
 * 
 * public void sortBy(SearchKeys sortBy)
 * Sorts the Json-file depending on sortBy-key, it gets re-written on the same Json-file.
 * parameters:
 *      sortBy - A structure type that encapsulates the sort by string.
 * 
 **/

namespace PRINCESS.controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        List<Review> reviews;

        public class SearchKeys
        {
            [Required]
            public string SearchString { get; set; }
            public string SearchType { get; set; }
            public string SortBy { get; set; }
        }

        // POST: api/Database
        [Authorize]
        [HttpPost]
        public void Post(SearchKeys searchKey)
        {

            switch (searchKey.SearchType)
            {
                case "Title":
                    reviews = new List<Review>(PrincessDb.searchReviewByTitle(searchKey.SearchString, searchKey.SortBy));
                    break;
                case "Domain":
                    reviews = new List<Review>(PrincessDb.searchReviewByDomain(searchKey.SearchString, searchKey.SortBy));
                    break;
                case "Writer":
                    reviews = new List<Review>(PrincessDb.searchReviewByWriter(searchKey.SearchString, searchKey.SortBy));
                    break;
                default:
                    reviews = new List<Review>(PrincessDb.searchReviewByTitle(searchKey.SearchString, searchKey.SortBy));
                    break;
            }

            string JSONresult = JsonConvert.SerializeObject(reviews);
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

        }

        // POST: api/Database/sortBy
        [Authorize]
        [HttpPost("sortBy")]
        public void sortBy([FromBody] string sortBy)
        {
            string path = @"C:\PRINCESS\PRINCESS\princess\src\components\JsonData\reviews.json";
            List<Review> jsonRev;
            IEnumerable<Review> sortedList;

            jsonRev = ReadFromJSON(path);
            if (jsonRev != null)
            {
                switch (sortBy)
                {
                    case "Priority":
                        sortedList = jsonRev.OrderBy(x => x.Url);
                        WriteToJSON(sortedList.ToList<Review>(), path);
                        break;
                }
            }
        }

        private List<Review> ReadFromJSON(string path)
        {
            List<Review> jsonRev;
            if (System.IO.File.Exists(path))
            {
                using (var tw = new StreamReader(path, true))
                {
                    string json = tw.ReadToEnd();
                    jsonRev = JsonConvert.DeserializeObject<List<Review>>(json);
                    tw.Close();
                }
                return jsonRev;
            }
            else
            {
                return null;
            }
        }

        private void WriteToJSON(List<Review> sortedList, string path)
        {
            string JSONresult = JsonConvert.SerializeObject(sortedList);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(JSONresult.ToString());
                    tw.Close();
                }
            }
        }
    }
}
